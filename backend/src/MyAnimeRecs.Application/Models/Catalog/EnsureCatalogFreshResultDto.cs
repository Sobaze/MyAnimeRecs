namespace MyAnimeRecs.Application.Models.Catalog;

public class EnsureCatalogFreshResultDto
{
    public string ActionTaken { get; set; } = string.Empty;
    public CatalogStatusDto Status { get; set; } = new();
    public CatalogSeedResultDto? SeedResult { get; set; }
}
