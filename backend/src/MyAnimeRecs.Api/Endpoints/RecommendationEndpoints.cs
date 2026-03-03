using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Recommendations;

namespace MyAnimeRecs.Api.Endpoints;

public static class RecommendationEndpoints
{
    public static IEndpointRouteBuilder MapRecommendationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/recommendations/by-genres", async (RecommendByGenresRequest request, IRecommendationService recommendationService, CancellationToken cancellationToken) =>
            {
                var result = await recommendationService.RecommendByGenresAsync(request, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("RecommendByGenres")
            .WithOpenApi();

        app.MapPost("/api/recommendations/by-mal/{username}", async (string username, int? maxItems, IRecommendationService recommendationService, CancellationToken cancellationToken) =>
            {
                var result = await recommendationService.RecommendByMalUsernameAsync(
                    new RecommendByMalUsernameRequest
                    {
                        Username = username,
                        MaxItems = maxItems ?? 10
                    },
                    cancellationToken);

                return Results.Ok(result);
            })
            .WithName("RecommendByMalUsername")
            .WithOpenApi();

        app.MapPost("/api/recommendations/for-friend/{username}", async (string username, RecommendForFriendRequest request, IRecommendationService recommendationService, CancellationToken cancellationToken) =>
            {
                var result = await recommendationService.RecommendFromCompletedForFriendAsync(username, request, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("RecommendFromCompletedForFriend")
            .WithOpenApi();

        app.MapGet("/api/recommendations/random/{username}", async (string username, int? count, IRecommendationService recommendationService, CancellationToken cancellationToken) =>
            {
                var result = await recommendationService.RecommendRandomUnseenForUserAsync(username, count ?? 3, cancellationToken);
                if (result.Count == 0)
                {
                    return Results.NotFound(new { message = "No unseen recommendations found for this user." });
                }

                return Results.Ok(result);
            })
            .WithName("RecommendRandomUnseen")
            .WithOpenApi();

        return app;
    }
}
