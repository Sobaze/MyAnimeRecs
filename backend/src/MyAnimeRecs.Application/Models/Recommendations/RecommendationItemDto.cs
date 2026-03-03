namespace MyAnimeRecs.Application.Models.Recommendations;

public class RecommendationItemDto
{
    public Guid AnimeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? MainPictureMediumUrl { get; set; }
    public string? MainPictureLargeUrl { get; set; }
    public decimal Score { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string SourceType { get; set; } = string.Empty;
    public string SourceAnimeId { get; set; } = string.Empty;
}
