using Microsoft.EntityFrameworkCore;
using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Import;
using MyAnimeRecs.Application.Models.Recommendations;
using MyAnimeRecs.Domain.Entities;

namespace MyAnimeRecs.Infrastructure.Services;

public class RecommendationService(IApplicationDbContext dbContext, IAnimeImportService animeImportService) : IRecommendationService
{
    public async Task<IReadOnlyCollection<RecommendationItemDto>> RecommendByGenresAsync(RecommendByGenresRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedGenres = request.Genres
            .Select(NormalizeGenreName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (normalizedGenres.Count == 0)
        {
            return Array.Empty<RecommendationItemDto>();
        }

        var matchedGenreIds = await dbContext.Genres
            .Where(x => normalizedGenres.Contains(x.NormalizedName))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (matchedGenreIds.Count == 0)
        {
            return Array.Empty<RecommendationItemDto>();
        }

        var seenAnimeIds = new HashSet<Guid>();
        if (request.ExcludeSeen && !string.IsNullOrWhiteSpace(request.Username))
        {
            var seenAnimeList = await dbContext.UserAnimeEntries
                .Where(x => x.UserProfile.Username == request.Username)
                .Select(x => x.AnimeId)
                .ToListAsync(cancellationToken);

            seenAnimeIds = seenAnimeList.ToHashSet();
        }

        var candidates = await dbContext.Animes
            .Where(x => x.AnimeGenres.Any(g => matchedGenreIds.Contains(g.GenreId)))
            .Select(x => new
            {
                Anime = x,
                MatchedGenreNames = x.AnimeGenres
                    .Where(g => matchedGenreIds.Contains(g.GenreId))
                    .Select(g => g.Genre.Name)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return candidates
            .Where(x => !seenAnimeIds.Contains(x.Anime.Id))
            .Select(x => new RecommendationItemDto
            {
                AnimeId = x.Anime.Id,
                Title = x.Anime.Title,
                MainPictureMediumUrl = x.Anime.MainPictureMediumUrl,
                MainPictureLargeUrl = x.Anime.MainPictureLargeUrl,
                SourceType = x.Anime.SourceType.ToString(),
                SourceAnimeId = x.Anime.SourceAnimeId,
                Score = x.MatchedGenreNames.Count,
                Reason = $"Matched genres: {string.Join(", ", x.MatchedGenreNames)}"
            })
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Title)
            .Take(Math.Max(1, request.MaxItems))
            .ToList();
    }

    public async Task<IReadOnlyCollection<RecommendationItemDto>> RecommendByMalUsernameAsync(RecommendByMalUsernameRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            return Array.Empty<RecommendationItemDto>();
        }

        await animeImportService.ImportCompletedFromMalUsernameAsync(
            new ImportFromMalUsernameRequest { Username = request.Username },
            cancellationToken);

        var userProfileId = await dbContext.UserProfiles
            .Where(x => x.Username == request.Username)
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);

        var watchedAnimeList = await dbContext.UserAnimeEntries
            .Where(x => x.UserProfileId == userProfileId)
            .Select(x => x.AnimeId)
            .ToListAsync(cancellationToken);

        var watchedAnimeIds = watchedAnimeList.ToHashSet();

        var genreWeights = await dbContext.UserAnimeEntries
            .Where(x => x.UserProfileId == userProfileId && x.Status == AnimeListStatus.Completed)
            .SelectMany(x => x.Anime.AnimeGenres.Select(g => g.GenreId))
            .GroupBy(x => x)
            .Select(g => new { GenreId = g.Key, Weight = g.Count() })
            .ToDictionaryAsync(x => x.GenreId, x => x.Weight, cancellationToken);

        if (genreWeights.Count == 0)
        {
            return Array.Empty<RecommendationItemDto>();
        }

        var candidates = await dbContext.Animes
            .Where(x => !watchedAnimeIds.Contains(x.Id) && x.AnimeGenres.Any(g => genreWeights.Keys.Contains(g.GenreId)))
            .Select(x => new
            {
                Anime = x,
                MatchedGenres = x.AnimeGenres
                    .Where(g => genreWeights.Keys.Contains(g.GenreId))
                    .Select(g => new { g.GenreId, g.Genre.Name })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return candidates
            .Select(x => new RecommendationItemDto
            {
                AnimeId = x.Anime.Id,
                Title = x.Anime.Title,
                MainPictureMediumUrl = x.Anime.MainPictureMediumUrl,
                MainPictureLargeUrl = x.Anime.MainPictureLargeUrl,
                SourceType = x.Anime.SourceType.ToString(),
                SourceAnimeId = x.Anime.SourceAnimeId,
                Score = x.MatchedGenres.Sum(g => genreWeights[g.GenreId]),
                Reason = $"Based on your completed-list genres: {string.Join(", ", x.MatchedGenres.Select(g => g.Name).Distinct())}"
            })
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Title)
            .Take(Math.Max(1, request.MaxItems))
            .ToList();
    }

    public async Task<IReadOnlyCollection<RecommendationItemDto>> RecommendFromCompletedForFriendAsync(string username, RecommendForFriendRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim();
        if (string.IsNullOrWhiteSpace(normalizedUsername))
        {
            return Array.Empty<RecommendationItemDto>();
        }

        var normalizedGenres = request.Genres
            .Select(NormalizeGenreName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (normalizedGenres.Count == 0)
        {
            return Array.Empty<RecommendationItemDto>();
        }

        var matchedGenreIds = await dbContext.Genres
            .Where(x => normalizedGenres.Contains(x.NormalizedName))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (matchedGenreIds.Count == 0)
        {
            return Array.Empty<RecommendationItemDto>();
        }

        var candidates = await dbContext.UserAnimeEntries
            .Where(x => x.UserProfile.Username == normalizedUsername && x.Status == AnimeListStatus.Completed)
            .Select(x => new
            {
                Anime = x.Anime,
                UserScore = x.Score,
                MatchedGenreNames = x.Anime.AnimeGenres
                    .Where(g => matchedGenreIds.Contains(g.GenreId))
                    .Select(g => g.Genre.Name)
                    .ToList()
            })
            .Where(x => x.MatchedGenreNames.Count > 0)
            .ToListAsync(cancellationToken);

        return candidates
            .Select(x => new RecommendationItemDto
            {
                AnimeId = x.Anime.Id,
                Title = x.Anime.Title,
                MainPictureMediumUrl = x.Anime.MainPictureMediumUrl,
                MainPictureLargeUrl = x.Anime.MainPictureLargeUrl,
                SourceType = x.Anime.SourceType.ToString(),
                SourceAnimeId = x.Anime.SourceAnimeId,
                Score = (x.MatchedGenreNames.Count * 2m) + (((x.UserScore ?? 0m) / 10m) * 8m),
                Reason = $"From your completed list. Your score: {(x.UserScore?.ToString("0.##") ?? "not rated")}/10. Matched genres: {string.Join(", ", x.MatchedGenreNames.Distinct())}"
            })
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Title)
            .Take(Math.Max(1, request.MaxItems))
            .ToList();
    }

    public async Task<IReadOnlyCollection<RecommendationItemDto>> RecommendRandomUnseenForUserAsync(string username, int count = 3, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Array.Empty<RecommendationItemDto>();
        }

        var candidates = await RecommendByMalUsernameAsync(
            new RecommendByMalUsernameRequest { Username = username, MaxItems = 100 },
            cancellationToken);

        var candidateList = candidates.ToList();
        if (candidateList.Count == 0)
        {
            return Array.Empty<RecommendationItemDto>();
        }

        var targetCount = Math.Clamp(count, 1, 10);
        var shuffled = candidateList.OrderBy(_ => Random.Shared.Next()).ToList();
        return shuffled.Take(targetCount).ToList();
    }

    private static string NormalizeGenreName(string value) => value.Trim().ToLowerInvariant();
}
