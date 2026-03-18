using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Catalog;

namespace MyAnimeRecs.Api.Endpoints;

public static class CatalogEndpoints
{
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/catalog/status", async (IAnimeCatalogService catalogService, CancellationToken cancellationToken) =>
            {
                var result = await catalogService.GetStatusAsync(cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetCatalogStatus")
            .WithOpenApi();

        app.MapPost("/api/catalog/ensure-fresh", async (IAnimeCatalogService catalogService, CancellationToken cancellationToken) =>
            {
                var result = await catalogService.EnsureFreshAsync(cancellationToken);
                return Results.Ok(result);
            })
            .WithName("EnsureCatalogFresh")
            .WithOpenApi();

        app.MapPost("/api/catalog/seed", async (CatalogSeedRequest request, IAnimeCatalogService catalogService, CancellationToken cancellationToken) =>
            {
                var result = await catalogService.SeedCatalogAsync(request, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("SeedAnimeCatalog")
            .WithOpenApi();

        return app;
    }
}
