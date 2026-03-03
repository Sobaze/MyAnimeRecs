using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyAnimeRecs.Application.Abstractions;
using MyAnimeRecs.Infrastructure.External.Mal;
using MyAnimeRecs.Infrastructure.Persistence;
using MyAnimeRecs.Infrastructure.Services;

namespace MyAnimeRecs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, string malClientId)
    {
        services.AddDbContext<MyAnimeDBContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<MyAnimeDBContext>());

        services.AddHttpClient<IMalClient, MalClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.myanimelist.net/v2/");
            client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", malClientId);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddScoped<IAnimeImportService, AnimeImportService>();
        services.AddScoped<IRecommendationService, RecommendationService>();
        services.AddScoped<IAnimeListQueryService, AnimeListQueryService>();

        return services;
    }
}
