namespace MyAnimeRecs.Application.Models.Catalog;

public class CatalogSeedResultDto
{
    public int TotalProcessed { get; set; }
    public int CreatedAnimes { get; set; }
    public int UpdatedAnimes { get; set; }
    public int CreatedGenres { get; set; }
    public int CreatedAnimeGenreLinks { get; set; }
}
