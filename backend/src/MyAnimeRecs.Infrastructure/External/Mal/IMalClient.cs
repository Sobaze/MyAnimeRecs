namespace MyAnimeRecs.Infrastructure.External.Mal;

public interface IMalClient
{
    Task<IReadOnlyCollection<MalAnimeListItem>> GetUserAnimeListAsync(string username, string status, CancellationToken cancellationToken = default);
    Task<MalRankingResponse> GetAnimeRankingPageAsync(string rankingType, int limit, int offset, CancellationToken cancellationToken = default);
}
