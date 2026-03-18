namespace MyAnimeRecs.Application.Models.Catalog;

public class CatalogStatusDto
{
    public bool IsSeeded { get; set; }
    public int TotalCatalogAnime { get; set; }
    public DateTime? LastRunAtUtc { get; set; }
    public DateTime? LastSuccessAtUtc { get; set; }
    public bool NeedsRefresh { get; set; }
    public string? LastError { get; set; }
}
