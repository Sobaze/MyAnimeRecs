using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Application.Models.Import;

namespace MyAnimeRecs.Api.Endpoints;

public static class ImportEndpoints
{
    public static IEndpointRouteBuilder MapImportEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/import/mal/{username}", async (string username, IAnimeImportService importService, CancellationToken cancellationToken) =>
            {
                var result = await importService.ImportCompletedFromMalUsernameAsync(
                    new ImportFromMalUsernameRequest { Username = username },
                    cancellationToken);

                return Results.Ok(result);
            })
            .WithName("ImportMalCompleted")
            .WithOpenApi();

        return app;
    }
}
