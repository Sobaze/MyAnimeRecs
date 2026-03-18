using MyAnimeRecs.Domain.Entities;

namespace MyAnimeRecs.Application.Abstractions;

public interface IApplicationDbContext
{
    IQueryable<Anime> Animes { get; }
    IQueryable<UserProfile> UserProfiles { get; }
    IQueryable<Genre> Genres { get; }
    IQueryable<AnimeGenre> AnimeGenres { get; }
    IQueryable<UserAnimeEntry> UserAnimeEntries { get; }
    IQueryable<Recommendation> Recommendations { get; }
    IQueryable<CatalogSyncState> CatalogSyncStates { get; }

    void Add<T>(T entity) where T : class;
    void AddRange<T>(IEnumerable<T> entities) where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
