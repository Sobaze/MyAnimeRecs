namespace MyAnimeRecs.Domain.Entities;

public class AnimeGenre
{
    public Guid AnimeId { get; set; }
    public Guid GenreId { get; set; }

    public Anime Anime { get; set; } = null!;
    public Genre Genre { get; set; } = null!;
}
