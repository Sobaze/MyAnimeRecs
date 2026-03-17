using System.Text.Json.Serialization;

namespace MyAnimeRecs.Infrastructure.External.Mal;

public sealed class MalAnimeListResponse
{
    [JsonPropertyName("data")]
    public List<MalAnimeListItem> Data { get; set; } = new();

    [JsonPropertyName("paging")]
    public MalPaging? Paging { get; set; }
}

public sealed class MalPaging
{
    [JsonPropertyName("next")]
    public string? Next { get; set; }
}

public sealed class MalAnimeListItem
{
    [JsonPropertyName("node")]
    public MalAnimeNode Node { get; set; } = new();

    [JsonPropertyName("list_status")]
    public MalListStatus? ListStatus { get; set; }
}

public sealed class MalAnimeNode
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("mean")]
    public decimal? Mean { get; set; }

    [JsonPropertyName("main_picture")]
    public MalMainPicture? MainPicture { get; set; }

    [JsonPropertyName("genres")]
    public List<MalGenre> Genres { get; set; } = new();
}

public sealed class MalMainPicture
{
    [JsonPropertyName("medium")]
    public string? Medium { get; set; }

    [JsonPropertyName("large")]
    public string? Large { get; set; }
}

public sealed class MalGenre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public sealed class MalListStatus
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("score")]
    public decimal? Score { get; set; }
}

public sealed class MalRankingResponse
{
    [JsonPropertyName("data")]
    public List<MalRankingAnimeItem> Data { get; set; } = new();

    [JsonPropertyName("paging")]
    public MalPaging? Paging { get; set; }
}

public sealed class MalRankingAnimeItem
{
    [JsonPropertyName("node")]
    public MalAnimeNode Node { get; set; } = new();
    [JsonPropertyName("ranking")]
    public MalRanking? Ranking { get; set; }
}