namespace MyAnimeRecs.Application.Models.Import;

public class ImportResultDto
{
    public string Username { get; set; } = string.Empty;
    public int ImportedCompletedEntries { get; set; }
    public int CreatedAnimeCount { get; set; }
    public int LinkedUserEntryCount { get; set; }
}
