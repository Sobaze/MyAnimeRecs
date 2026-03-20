using Microsoft.EntityFrameworkCore;
using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Users;
using MyAnimeRecs.Domain.Entities;

namespace MyAnimeRecs.Infrastructure.Services;

public class AnimeListQueryService(IApplicationDbContext dbContext) : IAnimeListQueryService
{
    public async Task<IReadOnlyCollection<UserAnimeListItemDto>> GetUserAnimeListAsync(string username, string? status = null, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim();
        var query = dbContext.UserAnimeEntries
            .Where(x => x.UserProfile.Username == normalizedUsername)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status) && TryParseStatus(status, out var parsedStatus))
        {
            query = query.Where(x => x.Status == parsedStatus);
        }

        return await query
            .OrderByDescending(x => x.UpdatedAtUtc)
            .Select(x => new UserAnimeListItemDto
            {
                AnimeId = x.AnimeId,
                Title = x.Anime.Title,
                MainPictureMediumUrl = x.Anime.MainPictureMediumUrl,
                MainPictureLargeUrl = x.Anime.MainPictureLargeUrl,
                Synopsis = x.Anime.Synopsis,
                Status = x.Status.ToString(),
                UserScore = x.Score,
                MeanScore = x.Anime.MeanScore,
                SourceAnimeId = x.Anime.SourceAnimeId,
                Genres = x.Anime.AnimeGenres.Select(g => g.Genre.Name).ToList()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TopGenreDto>> GetTopGenresAsync(string username, int limit = 10, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim();
        return await dbContext.UserAnimeEntries
            .Where(x => x.UserProfile.Username == normalizedUsername && x.Status == AnimeListStatus.Completed)
            .SelectMany(x => x.Anime.AnimeGenres.Select(g => g.Genre.Name))
            .GroupBy(x => x)
            .Select(g => new TopGenreDto
            {
                Name = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Name)
            .Take(Math.Max(1, limit))
            .ToListAsync(cancellationToken);
    }

    private static bool TryParseStatus(string value, out AnimeListStatus status)
    {
        var normalized = value.Trim().ToLowerInvariant();
        status = normalized switch
        {
            "completed" => AnimeListStatus.Completed,
            "watching" => AnimeListStatus.Watching,
            "on_hold" => AnimeListStatus.OnHold,
            "planned" => AnimeListStatus.Planned,
            "plan_to_watch" => AnimeListStatus.Planned,
            "dropped" => AnimeListStatus.Dropped,
            _ => default
        };

        return normalized is "completed" or "watching" or "on_hold" or "planned" or "plan_to_watch" or "dropped";
    }
}
