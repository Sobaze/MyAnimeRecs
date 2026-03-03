namespace MyAnimeRecs.Application.Models.Recommendations;

public class RecommendForFriendRequest
{
    public List<string> Genres { get; set; } = new();
    public int MaxItems { get; set; } = 10;
}
