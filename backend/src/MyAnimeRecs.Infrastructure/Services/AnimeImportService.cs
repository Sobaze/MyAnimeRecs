using Microsoft.EntityFrameworkCore;
using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Import;
using MyAnimeRecs.Domain.Entities;
using MyAnimeRecs.Infrastructure.External.Mal;

namespace MyAnimeRecs.Infrastructure.Services;

public class AnimeImportService(IApplicationDbContext dbContext, IMalClient malClient) : IAnimeImportService
{
    public async Task<ImportResultDto> ImportCompletedFromMalUsernameAsync(ImportFromMalUsernameRequest request, CancellationToken cancellationToken = default)
    {
        var username = request.Username.Trim();
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username is required.", nameof(request.Username));
        }

        var profile = await dbContext.UserProfiles.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        if (profile is null)
        {
            profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Username = username,
                CreatedAtUtc = DateTime.UtcNow
            };
            dbContext.Add(profile);
        }

        var completedEntries = await malClient.GetUserAnimeListAsync(username, "completed", cancellationToken);

        var genresByNormalizedName = await dbContext.Genres
            .ToDictionaryAsync(x => x.NormalizedName, StringComparer.OrdinalIgnoreCase, cancellationToken);

        var animeBySourceId = await dbContext.Animes
            .Where(x => x.SourceType == DataSourceType.MyAnimeList)
            .ToDictionaryAsync(x => x.SourceAnimeId, cancellationToken);

        var animeIds = animeBySourceId.Values.Select(x => x.Id).ToList();
        var existingAnimeGenreLinks = await dbContext.AnimeGenres
            .Where(x => animeIds.Contains(x.AnimeId))
            .Select(x => new { x.AnimeId, x.GenreId })
            .ToListAsync(cancellationToken);
        var animeGenreLinks = existingAnimeGenreLinks
            .Select(x => $"{x.AnimeId:N}:{x.GenreId:N}")
            .ToHashSet(StringComparer.Ordinal);

        var existingUserEntries = await dbContext.UserAnimeEntries
            .Where(x => x.UserProfileId == profile.Id)
            .ToDictionaryAsync(x => x.AnimeId, cancellationToken);

        var createdAnimeCount = 0;
        var linkedUserEntryCount = 0;
        foreach (var entry in completedEntries)
        {
            if (entry.Node.Id <= 0)
            {
                continue;
            }

            var sourceAnimeId = entry.Node.Id.ToString();
            animeBySourceId.TryGetValue(sourceAnimeId, out var anime);

            if (anime is null)
            {
                anime = new Anime
                {
                    Id = Guid.NewGuid(),
                    Title = entry.Node.Title,
                    SourceType = DataSourceType.MyAnimeList,
                    SourceAnimeId = sourceAnimeId,
                    MeanScore = entry.Node.Mean,
                    MainPictureMediumUrl = entry.Node.MainPicture?.Medium,
                    MainPictureLargeUrl = entry.Node.MainPicture?.Large,
                    CreatedAtUtc = DateTime.UtcNow
                };
                dbContext.Add(anime);
                animeBySourceId[sourceAnimeId] = anime;
                createdAnimeCount++;
            }

            anime.MeanScore = entry.Node.Mean;
            anime.MainPictureMediumUrl = entry.Node.MainPicture?.Medium;
            anime.MainPictureLargeUrl = entry.Node.MainPicture?.Large;
            if (!string.IsNullOrWhiteSpace(entry.Node.Title))
            {
                anime.Title = entry.Node.Title;
            }

            foreach (var externalGenre in entry.Node.Genres)
            {
                var genreName = externalGenre.Name.Trim();
                var normalizedGenreName = NormalizeGenreName(genreName);
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
                }

                var linkKey = BuildAnimeGenreLinkKey(anime.Id, genre.Id);
                if (!animeGenreLinks.Contains(linkKey))
                {
                    dbContext.Add(new AnimeGenre
                    {
                        AnimeId = anime.Id,
                        GenreId = genre.Id
                    });
                    animeGenreLinks.Add(linkKey);
                }
            }

            existingUserEntries.TryGetValue(anime.Id, out var existingUserEntry);
            if (existingUserEntry is null)
            {
                var newUserEntry = new UserAnimeEntry
                {
                    Id = Guid.NewGuid(),
                    UserProfileId = profile.Id,
                    AnimeId = anime.Id,
                    Status = AnimeListStatus.Completed,
                    Score = entry.ListStatus?.Score,
                    SourceType = DataSourceType.MyAnimeList,
                    SourceUserName = username,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow
                };

                dbContext.Add(newUserEntry);
                existingUserEntries[anime.Id] = newUserEntry;
                linkedUserEntryCount++;
            }
            else
            {
                existingUserEntry.Status = AnimeListStatus.Completed;
                existingUserEntry.Score = entry.ListStatus?.Score;
                existingUserEntry.SourceType = DataSourceType.MyAnimeList;
                existingUserEntry.SourceUserName = username;
                existingUserEntry.UpdatedAtUtc = DateTime.UtcNow;
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new ImportResultDto
        {
            Username = username,
            ImportedCompletedEntries = completedEntries.Count,
            CreatedAnimeCount = createdAnimeCount,
            LinkedUserEntryCount = linkedUserEntryCount
        };
    }

    private static string NormalizeGenreName(string value) => value.Trim().ToLowerInvariant();

    private static string BuildAnimeGenreLinkKey(Guid animeId, Guid genreId) => $"{animeId:N}:{genreId:N}";
}
