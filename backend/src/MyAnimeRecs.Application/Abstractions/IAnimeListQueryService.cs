using MyAnimeRecs.Application.Models.Users;

namespace MyAnimeRecs.Application.Abstractions;

public interface IAnimeListQueryService
{
    Task<IReadOnlyCollection<UserAnimeListItemDto>> GetUserAnimeListAsync(string username, string? status = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TopGenreDto>> GetTopGenresAsync(string username, int limit = 10, CancellationToken cancellationToken = default);
}
