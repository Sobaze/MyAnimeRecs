namespace MyAnimeRecs.Domain.Entities;

public class AnimeCatalog
{
    public Guid Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string SyncType { get; set; } = "ranking_all, ranking_bypopularity";
    public int SourceAnimeId { get; set; }
    public DataSourceType SourceType { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal? MeanScore { get; set; }
    public string? MainPictureMediumUrl { get; set; }
    public string? MainPictureLargeUrl { get; set; }
    public bool IsSeeded { get; set; } = false;
    public string? CatalogSource { get; set; }
    public int? Popularity { get; set; }
    public int? Rank { get; set; }
    public int? Episodes { get; set; }
    public string? Status { get; set; }
    public int? LastOffset { get; set; }
    public DateTime? LastUpdateUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string? LastError { get; set; }
}