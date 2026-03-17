using MyAnimeRecs.Application.Models.Catalog;

namespace MyAnimeRecs.Application.Abstractions;

public interface IAnimeCatalogService
{
    Task<CatalogSeedResultDto> SeedCatalogAsync(int animeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CatalogSeedRequest>> SearchAnimeAsync(string query, CancellationToken cancellationToken = default);
}