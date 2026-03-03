namespace MyAnimeRecs.Application.Models.Users;

public class UserAnimeListItemDto
{
    public Guid AnimeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? MainPictureMediumUrl { get; set; }
    public string? MainPictureLargeUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? UserScore { get; set; }
    public decimal? MeanScore { get; set; }
    public IReadOnlyCollection<string> Genres { get; set; } = Array.Empty<string>();
}
