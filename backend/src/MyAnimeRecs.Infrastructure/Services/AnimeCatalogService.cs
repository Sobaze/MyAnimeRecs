using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Catalog;
using MyAnimeRecs.Domain.Entities;
using MyAnimeRecs.Infrastructure.External.Mal;
using MyAnimeRecs.Infrastructure.Persistence;

namespace MyAnimeRecs.Infrastructure.Services;

public class AnimeCatalogService(MyAnimeDBContext dbContext, IMalClient malClient) : IAnimeCatalogService
{
    private static readonly SemaphoreSlim SeedLock = new(1, 1);
    private const string ProviderName = "mal";
    private const string CatalogLockSyncType = "catalog_lock";
    private static readonly string LeaseOwner = $"{Environment.MachineName}:{Guid.NewGuid():N}";
    private static readonly TimeSpan LeaseDuration = TimeSpan.FromMinutes(5);
    private const int TopPageRefreshCount = 5;
    private const int TopPageStartOffset = 0;

    internal static readonly TimeSpan CatalogRefreshInterval = TimeSpan.FromHours(24);

    public async Task<CatalogStatusDto> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        var totalCatalogAnime = await dbContext.Animes
            .CountAsync(x => x.IsCatalogSeeded, cancellationToken);

        var syncStates = await dbContext.CatalogSyncStates
            .Where(x => x.Provider == ProviderName)
            .ToListAsync(cancellationToken);

        var lastRunAtUtc = syncStates
            .Where(x => x.LastRunAtUtc.HasValue)
            .Select(x => x.LastRunAtUtc)
            .Max();

        var lastSuccessAtUtc = syncStates
            .Where(x => x.LastSuccessAtUtc.HasValue)
            .Select(x => x.LastSuccessAtUtc)
            .Max();

        var lastError = syncStates
            .Where(x => !string.IsNullOrWhiteSpace(x.LastError))
            .OrderByDescending(x => x.LastRunAtUtc)
            .Select(x => x.LastError)
            .FirstOrDefault();

        var needsRefresh = totalCatalogAnime == 0
            || !lastSuccessAtUtc.HasValue
            || lastSuccessAtUtc.Value <= DateTime.UtcNow.Subtract(CatalogRefreshInterval);

        return new CatalogStatusDto
        {
            IsSeeded = totalCatalogAnime > 0,
            TotalCatalogAnime = totalCatalogAnime,
            LastRunAtUtc = lastRunAtUtc,
            LastSuccessAtUtc = lastSuccessAtUtc,
            NeedsRefresh = needsRefresh,
            LastError = lastError
        };
    }

    public async Task<EnsureCatalogFreshResultDto> EnsureFreshAsync(CancellationToken cancellationToken = default)
    {
        var status = await GetStatusAsync(cancellationToken);
        if (!status.NeedsRefresh)
        {
            return new EnsureCatalogFreshResultDto
            {
                ActionTaken = "noop_up_to_date",
                Status = status
            };
        }

        if (!await SeedLock.WaitAsync(0, cancellationToken))
        {
            return new EnsureCatalogFreshResultDto
            {
                ActionTaken = "noop_refresh_in_progress",
                Status = status
            };
        }

        var leaseAcquired = false;
        try
        {
            leaseAcquired = await TryAcquireCatalogLeaseAsync(cancellationToken);
            if (!leaseAcquired)
            {
                return new EnsureCatalogFreshResultDto
                {
                    ActionTaken = "noop_refresh_in_progress",
                    Status = status
                };
            }

            var seedResult = await SeedCatalogCoreAsync(new CatalogSeedRequest { ForceRefresh = true }, cancellationToken);
            var refreshedStatus = await GetStatusAsync(cancellationToken);

            return new EnsureCatalogFreshResultDto
            {
                ActionTaken = "refreshed",
                Status = refreshedStatus,
                SeedResult = seedResult
            };
        }
        finally
        {
            if (leaseAcquired)
            {
                await ReleaseCatalogLeaseAsync(cancellationToken);
            }

            SeedLock.Release();
        }
    }

    public async Task<CatalogSeedResultDto> SeedCatalogAsync(CatalogSeedRequest request, CancellationToken cancellationToken = default)
    {
        await SeedLock.WaitAsync(cancellationToken);

        var leaseAcquired = false;
        try
        {
            leaseAcquired = await TryAcquireCatalogLeaseAsync(cancellationToken);
            if (!leaseAcquired)
            {
                throw new InvalidOperationException("Catalog refresh is already running.");
            }

            return await SeedCatalogCoreAsync(request, cancellationToken);
        }
        finally
        {
            if (leaseAcquired)
            {
                await ReleaseCatalogLeaseAsync(cancellationToken);
            }

            SeedLock.Release();
        }
    }

    private async Task<CatalogSeedResultDto> SeedCatalogCoreAsync(CatalogSeedRequest request, CancellationToken cancellationToken)
    {
        var rankingTypes = request.RankingTypes
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (rankingTypes.Count == 0)
        {
            rankingTypes = new List<string> { "all", "bypopularity" };
        }

        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var maxPagesPerType = Math.Max(1, request.MaxPagesPerType);

        var result = new CatalogSeedResultDto();

        var genresByNormalizedName = await dbContext.Genres
            .ToDictionaryAsync(x => x.NormalizedName, StringComparer.OrdinalIgnoreCase, cancellationToken);

        var malAnimesBySourceId = await dbContext.Animes
            .Where(x => x.SourceType == DataSourceType.MyAnimeList)
            .ToDictionaryAsync(x => x.SourceAnimeId, cancellationToken);

        var existingAnimeGenreLinks = await dbContext.AnimeGenres
            .Select(x => new { x.AnimeId, x.GenreId })
            .ToListAsync(cancellationToken);
        var animeGenreLinks = existingAnimeGenreLinks
            .Select(x => GenreHelpers.BuildAnimeGenreLinkKey(x.AnimeId, x.GenreId))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var rankingType in rankingTypes)
        {
            var syncType = $"ranking_{rankingType.ToLowerInvariant()}";
            var syncState = await GetOrCreateSyncStateAsync(syncType, cancellationToken);

            var minimumDeepOffset = TopPageRefreshCount * pageSize;
            var deepOffset = request.ForceRefresh
                ? minimumDeepOffset
                : Math.Max(syncState.LastOffset, minimumDeepOffset);
            var topRefreshResult = await ProcessRankingRangeAsync(
                rankingType,
                syncType,
                TopPageStartOffset,
                TopPageRefreshCount,
                pageSize,
                syncState,
                result,
                genresByNormalizedName,
                malAnimesBySourceId,
                animeGenreLinks,
                cancellationToken);

            var deepPagesToProcess = Math.Max(1, maxPagesPerType - topRefreshResult.ProcessedPages);
            var deepResult = await ProcessRankingRangeAsync(
                rankingType,
                syncType,
                deepOffset,
                deepPagesToProcess,
                pageSize,
                syncState,
                result,
                genresByNormalizedName,
                malAnimesBySourceId,
                animeGenreLinks,
                cancellationToken);

            if (deepResult.ReachedEnd)
            {
                syncState.LastOffset = TopPageRefreshCount * pageSize;
            }
            else
            {
                syncState.LastOffset = deepOffset + (deepResult.ProcessedPages * pageSize);
            }

            syncState.LastSuccessAtUtc = DateTime.UtcNow;
            syncState.LastError = null;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    private async Task<(int ProcessedPages, bool ReachedEnd)> ProcessRankingRangeAsync(
        string rankingType,
        string syncType,
        int startOffset,
        int pageCount,
        int pageSize,
        CatalogSyncState syncState,
        CatalogSeedResultDto result,
        Dictionary<string, Genre> genresByNormalizedName,
        Dictionary<string, Anime> malAnimesBySourceId,
        HashSet<string> animeGenreLinks,
        CancellationToken cancellationToken)
    {
        if (pageCount <= 0)
        {
            return (0, false);
        }

        var offset = Math.Max(0, startOffset);
        var processedPages = 0;

        for (var page = 0; page < pageCount; page++)
        {
            syncState.LastRunAtUtc = DateTime.UtcNow;

            try
            {
                var rankingPage = await malClient.GetAnimeRankingPageAsync(rankingType, pageSize, offset, cancellationToken);
                if (rankingPage.Data.Count == 0)
                {
                    return (processedPages, true);
                }

                foreach (var item in rankingPage.Data)
                {
                    if (item.Node.Id <= 0)
                    {
                        continue;
                    }

                    var sourceAnimeId = item.Node.Id.ToString();
                    if (!malAnimesBySourceId.TryGetValue(sourceAnimeId, out var anime))
                    {
                        anime = new Anime
                        {
                            Id = Guid.NewGuid(),
                            SourceType = DataSourceType.MyAnimeList,
                            SourceAnimeId = sourceAnimeId,
                            CreatedAtUtc = DateTime.UtcNow
                        };
                        dbContext.Add(anime);
                        malAnimesBySourceId[sourceAnimeId] = anime;
                        result.CreatedAnimes++;
                    }
                    else
                    {
                        result.UpdatedAnimes++;
                    }

                    anime.Title = string.IsNullOrWhiteSpace(item.Node.Title) ? anime.Title : item.Node.Title;
                    anime.MeanScore = item.Node.Mean;
                    anime.MainPictureMediumUrl = item.Node.MainPicture?.Medium;
                    anime.MainPictureLargeUrl = item.Node.MainPicture?.Large;
                    anime.Synopsis = item.Node.Synopsis;
                    anime.IsCatalogSeeded = true;
                    anime.CatalogSource = syncType;
                    anime.Popularity = item.Node.Popularity;
                    anime.Rank = item.Ranking?.Rank ?? item.Node.Rank;
                    anime.MediaType = item.Node.MediaType;
                    anime.AiringStatus = item.Node.Status;
                    anime.Episodes = item.Node.NumEpisodes;
                    anime.LastAnimeCatalogUpdateUtc = DateTime.UtcNow;

                    foreach (var externalGenre in item.Node.Genres)
                    {
                        var genreName = externalGenre.Name.Trim();
                        var normalizedGenreName = GenreHelpers.NormalizeGenreName(genreName);
                        if (string.IsNullOrWhiteSpace(normalizedGenreName))
                        {
                            continue;
                        }

                        if (!genresByNormalizedName.TryGetValue(normalizedGenreName, out var genre))
                        {
                            genre = new Genre
                            {
                                Id = Guid.NewGuid(),
                                Name = genreName,
                                NormalizedName = normalizedGenreName
                            };
                            dbContext.Add(genre);
                            genresByNormalizedName[normalizedGenreName] = genre;
                            result.CreatedGenres++;
                        }

                        var linkKey = GenreHelpers.BuildAnimeGenreLinkKey(anime.Id, genre.Id);
                        if (!animeGenreLinks.Contains(linkKey))
                        {
                            dbContext.Add(new AnimeGenre
                            {
                                AnimeId = anime.Id,
                                GenreId = genre.Id
                            });
                            animeGenreLinks.Add(linkKey);
                            result.CreatedAnimeGenreLinks++;
                        }
                    }

                    result.TotalProcessed++;
                }

                syncState.LastSuccessAtUtc = DateTime.UtcNow;
                syncState.LastError = null;
                await dbContext.SaveChangesAsync(cancellationToken);

                processedPages++;
                offset += pageSize;
                if (string.IsNullOrWhiteSpace(rankingPage.Paging?.Next))
                {
                    return (processedPages, true);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                syncState.LastError = ex.Message;
                await dbContext.SaveChangesAsync(cancellationToken);
                throw;
            }
        }

        return (processedPages, false);
    }

    private async Task<CatalogSyncState> GetOrCreateSyncStateAsync(string syncType, CancellationToken cancellationToken)
    {
        var syncState = await dbContext.CatalogSyncStates
            .FirstOrDefaultAsync(x => x.Provider == ProviderName && x.SyncType == syncType, cancellationToken);

        if (syncState is not null)
        {
            return syncState;
        }

        syncState = new CatalogSyncState
        {
            Id = Guid.NewGuid(),
            Provider = ProviderName,
            SyncType = syncType,
            LastOffset = 0
        };

        dbContext.Add(syncState);
        await dbContext.SaveChangesAsync(cancellationToken);
        return syncState;
    }

    private async Task<bool> TryAcquireCatalogLeaseAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var leaseExpiry = now.Add(LeaseDuration);

        var updatedRows = await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
UPDATE CatalogSyncStates
SET LeaseOwner = {LeaseOwner}, LeaseExpiresAtUtc = {leaseExpiry}, LastRunAtUtc = {now}
WHERE Provider = {ProviderName}
  AND SyncType = {CatalogLockSyncType}
  AND (LeaseExpiresAtUtc IS NULL OR LeaseExpiresAtUtc <= {now} OR LeaseOwner = {LeaseOwner});", cancellationToken);

        if (updatedRows > 0)
        {
            return true;
        }

        try
        {
            dbContext.Add(new CatalogSyncState
            {
                Id = Guid.NewGuid(),
                Provider = ProviderName,
                SyncType = CatalogLockSyncType,
                LastOffset = 0,
                LastRunAtUtc = now,
                LastSuccessAtUtc = now,
                LeaseOwner = LeaseOwner,
                LeaseExpiresAtUtc = leaseExpiry
            });

            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
        {
            updatedRows = await dbContext.Database.ExecuteSqlInterpolatedAsync($@"
UPDATE CatalogSyncStates
SET LeaseOwner = {LeaseOwner}, LeaseExpiresAtUtc = {leaseExpiry}, LastRunAtUtc = {now}
WHERE Provider = {ProviderName}
  AND SyncType = {CatalogLockSyncType}
  AND (LeaseExpiresAtUtc IS NULL OR LeaseExpiresAtUtc <= {now} OR LeaseOwner = {LeaseOwner});", cancellationToken);

            return updatedRows > 0;
        }
    }

    private Task ReleaseCatalogLeaseAsync(CancellationToken cancellationToken)
    {
        return dbContext.Database.ExecuteSqlInterpolatedAsync($@"
UPDATE CatalogSyncStates
SET LeaseOwner = NULL, LeaseExpiresAtUtc = NULL
WHERE Provider = {ProviderName}
  AND SyncType = {CatalogLockSyncType}
  AND LeaseOwner = {LeaseOwner};", cancellationToken);
    }
}
