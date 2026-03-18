using Microsoft.EntityFrameworkCore;
using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Catalog;
using MyAnimeRecs.Domain.Entities;
using MyAnimeRecs.Infrastructure.External.Mal;

namespace MyAnimeRecs.Infrastructure.Services;

public class AnimeCatalogService(IApplicationDbContext dbContext, IMalClient malClient) : IAnimeCatalogService
{
    private static readonly SemaphoreSlim SeedLock = new(1, 1);
    internal static readonly TimeSpan CatalogRefreshInterval = TimeSpan.FromHours(24);

    public async Task<CatalogStatusDto> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        var totalCatalogAnime = await dbContext.Animes
            .CountAsync(x => x.IsCatalogSeeded, cancellationToken);

        var syncStates = await dbContext.CatalogSyncStates
            .Where(x => x.Provider == "mal")
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

        try
        {
            var seedResult = await SeedCatalogAsync(new CatalogSeedRequest { ForceRefresh = true }, cancellationToken);
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
            SeedLock.Release();
        }
    }

    public async Task<CatalogSeedResultDto> SeedCatalogAsync(CatalogSeedRequest request, CancellationToken cancellationToken = default)
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
            var syncState = await dbContext.CatalogSyncStates
                .FirstOrDefaultAsync(x => x.Provider == "mal" && x.SyncType == syncType, cancellationToken);

            if (syncState is null)
            {
                syncState = new CatalogSyncState
                {
                    Id = Guid.NewGuid(),
                    Provider = "mal",
                    SyncType = syncType,
                    LastOffset = 0
                };
                dbContext.Add(syncState);
            }

            var offset = request.ForceRefresh ? 0 : syncState.LastOffset;

            for (var page = 0; page < maxPagesPerType; page++)
            {
                syncState.LastRunAtUtc = DateTime.UtcNow;

                try
                {
                    var rankingPage = await malClient.GetAnimeRankingPageAsync(rankingType, pageSize, offset, cancellationToken);
                    if (rankingPage.Data.Count == 0)
                    {
                        syncState.LastSuccessAtUtc = DateTime.UtcNow;
                        syncState.LastError = null;
                        break;
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

                    offset += pageSize;
                    syncState.LastOffset = offset;
                    syncState.LastSuccessAtUtc = DateTime.UtcNow;
                    syncState.LastError = null;

                    await dbContext.SaveChangesAsync(cancellationToken);

                    if (string.IsNullOrWhiteSpace(rankingPage.Paging?.Next))
                    {
                        break;
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
        }

        return result;
    }

}
