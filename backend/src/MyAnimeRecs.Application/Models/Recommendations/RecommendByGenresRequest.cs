namespace MyAnimeRecs.Application.Models.Recommendations;

public class RecommendByGenresRequest
{
    public List<string> Genres { get; set; } = new();
    public string? Username { get; set; }
    public bool ExcludeSeen { get; set; } = true;
    public int MaxItems { get; set; } = 10;
}
