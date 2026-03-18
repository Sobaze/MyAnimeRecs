using MyAnimeRecs.Application.Models.Catalog;

namespace MyAnimeRecs.Application.Abstractions;

public interface IAnimeCatalogService
{
    Task<CatalogSeedResultDto> SeedCatalogAsync(CatalogSeedRequest request, CancellationToken cancellationToken = default);
    Task<CatalogStatusDto> GetStatusAsync(CancellationToken cancellationToken = default);
    Task<EnsureCatalogFreshResultDto> EnsureFreshAsync(CancellationToken cancellationToken = default);
}
