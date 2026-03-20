export type ImportSummary = {
  username: string
  importedCompletedEntries: number
  createdAnimeCount: number
  linkedUserEntryCount: number
}

export type UserAnimeListItem = {
  animeId: string
  title: string
  mainPictureMediumUrl: string | null
  mainPictureLargeUrl: string | null
  synopsis: string | null
  status: string
  userScore: number | null
  meanScore: number | null
  genres: string[]
  sourceAnimeId: string 
}

export type AnimeListStatus = 'completed' | 'watching' | 'on_hold' | 'dropped' | 'plan_to_watch'

export type TopGenre = {
  name: string
  count: number
}

export type RecommendationItem = {
  animeId: string
  title: string
  mainPictureMediumUrl: string | null
  mainPictureLargeUrl: string | null
  synopsis: string | null
  meanScore: number | null
  score: number
  reason: string
  sourceType: string
  sourceAnimeId: string
}

export type RecommendForFriendRequest = {
  genres: string[]
  maxItems: number
}

export type ApiErrorResponse = {
  error: string
  message: string
  details?: string
}

export type ErrorPayload = {
  message?: string
  error?: string
  details?: string
}

export type CatalogStatus = {
  isSeeded: boolean
  totalCatalogAnime: number
  lastRunAtUtc: string | null
  lastSuccessAtUtc: string | null
  needsRefresh: boolean
  lastError: string | null
}

export type CatalogSeedResult = {
  totalProcessed: number
  createdAnimes: number
  updatedAnimes: number
  createdGenres: number
  createdAnimeGenreLinks: number
}

export type EnsureCatalogFreshResult = {
  actionTaken: string
  status: CatalogStatus
  seedResult: CatalogSeedResult | null
}
