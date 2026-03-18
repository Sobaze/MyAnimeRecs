namespace MyAnimeRecs.Application.Models.Catalog;

public class CatalogSeedRequest
{
    public List<string> RankingTypes { get; set; } = new() { "all", "bypopularity" };
    public int PageSize { get; set; } = 100;
    public int MaxPagesPerType { get; set; } = 10;
    public bool ForceRefresh { get; set; }
}
