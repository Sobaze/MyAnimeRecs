namespace MyAnimeRecs.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<UserAnimeEntry> UserAnimeEntries { get; set; } = new List<UserAnimeEntry>();
    public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
}
