namespace MyAnimeRecs.Application.Abstractions;

public interface IRecommendationPlanner
{
    Task<IReadOnlyCollection<Guid>> BuildRecommendationsForUserAsync(Guid userProfileId, CancellationToken cancellationToken = default);
    Task RecalculateAllRecommendationScoresAsync(CancellationToken cancellationToken = default);
}
