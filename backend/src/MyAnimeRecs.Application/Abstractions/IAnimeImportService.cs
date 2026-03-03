using MyAnimeRecs.Application.Models.Import;

namespace MyAnimeRecs.Application.Abstractions;

public interface IAnimeImportService
{
    Task<ImportResultDto> ImportCompletedFromMalUsernameAsync(ImportFromMalUsernameRequest request, CancellationToken cancellationToken = default);
}
