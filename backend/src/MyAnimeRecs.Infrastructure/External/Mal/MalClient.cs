using System.Net;
using System.Net.Http.Json;

namespace MyAnimeRecs.Infrastructure.External.Mal;

public class MalClient(HttpClient httpClient) : IMalClient
{
    public async Task<IReadOnlyCollection<MalAnimeListItem>> GetUserAnimeListAsync(string username, string status, CancellationToken cancellationToken = default)
    {
        var allItems = new List<MalAnimeListItem>();
        var nextUrl = $"users/{Uri.EscapeDataString(username)}/animelist?status={Uri.EscapeDataString(status)}&limit=1000&fields=list_status,mean,genres,main_picture";

        while (!string.IsNullOrWhiteSpace(nextUrl))
        {
            var response = await SendAndReadAsync(nextUrl, cancellationToken);
            allItems.AddRange(response.Data);
            nextUrl = response.Paging?.Next;
        }

        return allItems;
    }

    private async Task<MalAnimeListResponse> SendAndReadAsync(string url, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = await httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync(cancellationToken);
            var message = string.IsNullOrWhiteSpace(errorText)
                ? $"MAL API returned {(int)response.StatusCode} ({response.StatusCode})."
                : errorText;

            throw new MalApiException(response.StatusCode, message);
        }

        var payload = await response.Content.ReadFromJsonAsync<MalAnimeListResponse>(cancellationToken: cancellationToken);
        if (payload is null)
        {
            throw new MalApiException(HttpStatusCode.BadGateway, "MAL API returned an empty response.");
        }

        return payload;
    }
    public async Task<IReadOnlyCollection<MalRankingAnimeItem>> GetAnimeRankingAsync(string rankingType,int limit, int offset, CancellationToken cancellationToken = default)
    {
        var allItems = new List<MalRankingAnimeItem>();
        var nextUrl = $"anime/ranking?ranking_type={Uri.EscapeDataString(rankingType)}&limit={limit}&offset={offset}&fields=mean,genres,main_picture";

        while (!string.IsNullOrWhiteSpace(nextUrl))
        {
            var response = await SendAndReadRankingAsync(nextUrl, cancellationToken);
            allItems.AddRange(response.Data);
            nextUrl = response.Paging?.Next;
        }

        return allItems;
    }
}
