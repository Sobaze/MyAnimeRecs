using MyAnimeRecs.Application.Abstractions;

namespace MyAnimeRecs.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{username}/anime-list", async (string username, string? status, IAnimeListQueryService queryService, CancellationToken cancellationToken) =>
            {
                var result = await queryService.GetUserAnimeListAsync(username, status, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetUserAnimeList")
            .WithOpenApi();

        app.MapGet("/api/users/{username}/genres/top", async (string username, int? limit, IAnimeListQueryService queryService, CancellationToken cancellationToken) =>
            {
                var result = await queryService.GetTopGenresAsync(username, limit ?? 10, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetUserTopGenres")
            .WithOpenApi();

        return app;
    }
}
