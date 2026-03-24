import { apiEndpoints } from './endpoints'
import type {
  ImportSummary,
  AnimeListStatus,
  CatalogStatus,
  EnsureCatalogFreshResult,
  RecommendForFriendRequest,
  RecommendationItem,
  TopGenre,
  UserAnimeListItem,
  ErrorPayload,
} from '../types/api'
import { ApiError } from './errors'

// Import a user's anime list from MyAnimeList. The response includes a summary of the import process, such as the number of entries imported and linked.
export async function importMalUser(username: string): Promise<ImportSummary> {
    const response = await fetch(apiEndpoints.importMalUser(username), {
      method:'POST',
      headers: {
        'Accept': 'application/json',
      },
    })
    const data = await parseJsonResponse<ImportSummary>(response)
    return data
}

export async function getCatalogStatus(): Promise<CatalogStatus> {
  const response = await fetch(apiEndpoints.getCatalogStatus(), {
    method: 'GET',
    headers: {
      Accept: 'application/json',
    },
  })

  return parseJsonResponse<CatalogStatus>(response)
}

export async function ensureCatalogFresh(): Promise<EnsureCatalogFreshResult> {
  const response = await fetch(apiEndpoints.ensureCatalogFresh(), {
    method: 'POST',
    headers: {
      Accept: 'application/json',
    },
  })

  return parseJsonResponse<EnsureCatalogFreshResult>(response)
}

// Get the user's anime list with the specified status (default is 'completed'). The response includes details about each anime in the list, 
// such as the title, main picture URLs, user score, mean score, and genres.
export async function getUserAnimeList(
  username: string,
  status: AnimeListStatus,
): Promise<UserAnimeListItem[]> {
  const response = await fetch(apiEndpoints.getUserAnimeList(username, status), {
    method: 'GET',
    headers: {
      'Accept': 'application/json',
    },
  })
  const data = await parseJsonResponse<UserAnimeListItem[]>(response)
  return data
}

// Get the top genres for a user based on their completed anime list. The response includes the genre name and the count of completed anime in that genre.
export async function getUserTopGenres(username: string, limit = 10): Promise<TopGenre[]> {
  const response = await fetch(apiEndpoints.getUserTopGenres(username, limit), {
    method: 'GET',
    headers: {
      'Accept': 'application/json',
    },
  })
  const data = await parseJsonResponse<TopGenre[]>(response)
  return data
}

// Recommend anime to a user based on my completed anime list and genres friend likes.
// The request body should include the genres the friend likes and the maximum number of recommendations to return.
export async function recommendForFriend(
  username: string,
  request: RecommendForFriendRequest,
): Promise<RecommendationItem[]> {
  const response = await fetch(apiEndpoints.recommendForFriend(username), {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    },
    body: JSON.stringify(request),
  })
  const data = await parseJsonResponse<RecommendationItem[]>(response)
  return data
}
//Recommend new anime that the user hasn't seen yet, based on their completed anime list. This endpoint will return recommendations that are not in the user's anime list at all, so it can include anime that the user has already started but not completed.
export async function recommendNewForUser(username: string, maxItems: number): Promise<RecommendationItem[]> {
  const response = await fetch(`${apiEndpoints.recommendNewForUser(username)}?maxItems=${maxItems}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    },
  })
  const data = await parseJsonResponse<RecommendationItem[]>(response)
  return data
}
// Recommend random anime that the user hasn't seen yet, based on their completed anime list. 
export async function recommendRandom(username: string, count = 3): Promise<RecommendationItem[]> {
  const response = await fetch(apiEndpoints.recommendRandom(username, count), {
    method: 'GET',
    headers: {
      'Accept': 'application/json',
    },
  })
  const data = await parseJsonResponse<RecommendationItem[]>(response)
  return data
}

// Utility function to parse JSON responses and handle errors
async function parseJsonResponse<T>(
  response: Response,
): Promise<T> {
  const text = await response.text()

  let data: unknown = null

  if(text) {
    try {
      data = JSON.parse(text)
    } catch {
      data = null
    }
  }
    
// If the response is not OK, try to extract error details from the response body
  if (!response.ok) {
    const payload = isErrorPayload(data) ? data : undefined
    const fallbackMessage = text && !data ? text : `Request failed with status ${response.status}`
    const message = payload?.message ?? fallbackMessage
    throw new ApiError(message, response.status, payload?.error, payload?.details)
  }
  if (data === null) {
    throw new ApiError('Response body is empty or not valid JSON.', response.status)
  }

  return data as T
}
// Type guard to check if the parsed JSON has the expected error structure
function isErrorPayload(value: unknown): value is ErrorPayload {
  return (
    typeof value === 'object' &&
    value !== null &&
    (!('message' in value) || typeof (value as Record<string, unknown>).message === 'string') &&
    (!('error' in value) || typeof (value as Record<string, unknown>).error === 'string') &&
    (!('details' in value) || typeof (value as Record<string, unknown>).details === 'string')
  )
}

export const api = {
  getCatalogStatus,
  ensureCatalogFresh,
  importMalUser,
  getUserAnimeList,
  getUserTopGenres,
  recommendForFriend,
  recommendRandom,
  recommendNewForUser
}
