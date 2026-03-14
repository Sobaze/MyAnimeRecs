import { useState } from 'react'
import type { RecommendationItem } from '../../types/api'
import { api } from '../../api/client'
import { ApiError } from '../../api/errors'
import { normalizeUsername } from '../../utils/normalize'
import { dedupeGenres } from '../../utils/genre'

type UseFriendRecommendationsResult = {
  isLoading: boolean
  error: string | null
  items: RecommendationItem[]
  load: (username: string, genres: string[], maxItems?: number) => Promise<void>
}

export function useFriendRecommendations(): UseFriendRecommendationsResult {
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [items, setItems] = useState<RecommendationItem[]>([])

  async function load(username: string, genres: string[], maxItems = 10): Promise<void> {
    const normalizedUsername = normalizeUsername(username)
    if (!normalizedUsername) {
      setError('Username is required.')
      setItems([])
      return
    }
    const dedupedGenres = dedupeGenres(genres)
    if (dedupedGenres.length === 0) {
      setError('At least one valid genre is required.')
      setItems([])
      return
    }

    const maxAmount = maxItems > 0 ? maxItems : 10
    setIsLoading(true)
    try {
      const response = await api.recommendForFriend(normalizedUsername, {
        genres: dedupedGenres,
        maxItems: maxAmount,
      })
      setItems(response)
    } catch (error: unknown) {
      if (error instanceof ApiError) {
        setError(error.message)
      } else if (error instanceof Error) {
        setError(error.message)
      } else {
        setError('Unexpected error occurred while loading friend recommendations.')
      }
    } finally {
      setIsLoading(false)
    }
  }

  return {
    isLoading,
    error,
    items,
    load,
  }
}
