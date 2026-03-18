namespace MyAnimeRecs.Infrastructure.Services;

internal static class GenreHelpers
{
    internal static string NormalizeGenreName(string value) => value.Trim().ToLowerInvariant();

    internal static string BuildAnimeGenreLinkKey(Guid animeId, Guid genreId) => $"{animeId:N}:{genreId:N}";
}
