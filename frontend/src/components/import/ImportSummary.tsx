import type { ImportSummary as ImportSummaryType } from '../../types/api'

type ImportSummaryProps = {
  summary: ImportSummaryType | null
}

export function ImportSummary({ summary }: ImportSummaryProps) {
  if (!summary) {
    return null
  }

  return (
    <div>
      <p>User: {summary.username}</p>
      <p>Imported: {summary.importedCompletedEntries}</p>
      <p>New anime: {summary.createdAnimeCount}</p>
      <p>Linked entries: {summary.linkedUserEntryCount}</p>
    </div>
  )
}
