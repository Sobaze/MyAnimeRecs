// For raw comma-separated text input (e.g. a text field)
export function parseGenreInput(input: string): string[] {
  return dedupeGenres(input.split(','))
}

// For an already-split array of genres (e.g. checkboxes, radio buttons, or an array from a hook)
export function dedupeGenres(genres: string[]): string[] {
  const seen = new Set<string>()
  const result: string[] = []

  for (const rawGenre of genres) {
    const genre = rawGenre.trim()
    if (!genre) {
      continue
    }

    const normalized = genre.toLowerCase()
    if (seen.has(normalized)) {
      continue
    }

    seen.add(normalized)
    result.push(genre)
  }

  return result
}
