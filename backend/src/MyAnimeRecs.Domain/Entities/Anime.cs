namespace MyAnimeRecs.Domain.Entities;

public class Anime
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DataSourceType SourceType { get; set; }
    public string SourceAnimeId { get; set; } = string.Empty;
    public decimal? MeanScore { get; set; }
    public string? MainPictureMediumUrl { get; set; }
    public string? MainPictureLargeUrl { get; set; }
    public bool IsCatalogSeeded { get; set; } = false;
    public string? CatalogSource { get; set; }
    public int? Popularity { get; set; }
    public int? Rank { get; set; }
    public int? Episodes { get; set; }
    public string? MediaType { get; set; }
    public string? AiringStatus { get; set; }
    public DateTime? LastAnimeCatalogUpdateUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<AnimeGenre> AnimeGenres { get; set; } = new List<AnimeGenre>();
    public ICollection<UserAnimeEntry> UserAnimeEntries { get; set; } = new List<UserAnimeEntry>();
    public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
}
