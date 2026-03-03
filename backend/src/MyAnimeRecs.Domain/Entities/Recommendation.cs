namespace MyAnimeRecs.Domain.Entities;

public class Recommendation
{
    public Guid Id { get; set; }
    public Guid UserProfileId { get; set; }
    public Guid AnimeId { get; set; }
    public decimal Score { get; set; }
    public string? Reason { get; set; }
    public string AlgorithmVersion { get; set; } = "v1";
    public DateTime CreatedAtUtc { get; set; }

    public UserProfile UserProfile { get; set; } = null!;
    public Anime Anime { get; set; } = null!;
}
