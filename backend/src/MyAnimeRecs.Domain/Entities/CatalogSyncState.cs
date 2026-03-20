namespace MyAnimeRecs.Domain.Entities;

public class CatalogSyncState
{
    public Guid Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string SyncType { get; set; } = string.Empty;
    public int LastOffset { get; set; }
    public DateTime? LastRunAtUtc { get; set; }
    public DateTime? LastSuccessAtUtc { get; set; }
    public string? LastError { get; set; }
    public string? LeaseOwner { get; set; }
    public DateTime? LeaseExpiresAtUtc { get; set; }
}
