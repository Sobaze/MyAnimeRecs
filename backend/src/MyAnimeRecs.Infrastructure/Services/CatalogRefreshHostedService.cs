using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyAnimeRecs.Application.Abstractions;

namespace MyAnimeRecs.Infrastructure.Services;

public class CatalogRefreshHostedService(IServiceProvider serviceProvider, ILogger<CatalogRefreshHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await EnsureFreshInternalAsync(stoppingToken);

        using var timer = new PeriodicTimer(AnimeCatalogService.CatalogRefreshInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await EnsureFreshInternalAsync(stoppingToken);
        }
    }

    private async Task EnsureFreshInternalAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var catalogService = scope.ServiceProvider.GetRequiredService<IAnimeCatalogService>();
            var result = await catalogService.EnsureFreshAsync(cancellationToken);
            logger.LogInformation("Catalog ensure-fresh action: {Action}", result.ActionTaken);
        }
        catch (OperationCanceledException)
        {
            // expected during shutdown
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Catalog background refresh failed");
        }
    }
}
