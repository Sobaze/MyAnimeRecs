using MyAnimeRecs.Application.Models.Recommendations;
using MyAnimeRecs.Domain.Entities;

namespace MyAnimeRecs.Infrastructure.Services;

internal static class RecommendationMappers
{
    public static RecommendationItemDto ToRecommendationDto(this Anime anime, decimal score, string reason)
    {
        return new RecommendationItemDto
        {
            AnimeId = anime.Id,
            Title = anime.Title,
            MainPictureMediumUrl = anime.MainPictureMediumUrl,
            MainPictureLargeUrl = anime.MainPictureLargeUrl,
            Synopsis = anime.Synopsis,
            MeanScore = anime.MeanScore,
            SourceType = anime.SourceType.ToString(),
            SourceAnimeId = anime.SourceAnimeId,
            Score = score,
            Reason = reason
        };
    }
}
