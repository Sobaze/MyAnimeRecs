namespace MyAnimeRecs.Domain.Entities;

public class UserAnimeEntry
{
    public Guid Id { get; set; }
    public Guid UserProfileId { get; set; }
    public Guid AnimeId { get; set; }
    public AnimeListStatus Status { get; set; }
    public decimal? Score { get; set; }
    public DataSourceType SourceType { get; set; }
    public string SourceUserName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public UserProfile UserProfile { get; set; } = null!;
    public Anime Anime { get; set; } = null!;
}
