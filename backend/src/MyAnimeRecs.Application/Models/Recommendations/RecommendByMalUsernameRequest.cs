namespace MyAnimeRecs.Application.Models.Recommendations;

public class RecommendByMalUsernameRequest
{
    public string Username { get; set; } = string.Empty;
    public int MaxItems { get; set; } = 10;
}
