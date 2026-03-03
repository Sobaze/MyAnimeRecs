using MyAnimeRecs.Application.Models.Recommendations;

namespace MyAnimeRecs.Application.Abstractions;

public interface IRecommendationService
{
    Task<IReadOnlyCollection<RecommendationItemDto>> RecommendByGenresAsync(RecommendByGenresRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RecommendationItemDto>> RecommendByMalUsernameAsync(RecommendByMalUsernameRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RecommendationItemDto>> RecommendFromCompletedForFriendAsync(string username, RecommendForFriendRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RecommendationItemDto>> RecommendRandomUnseenForUserAsync(string username, int count = 3, CancellationToken cancellationToken = default);
}
