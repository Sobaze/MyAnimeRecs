import { useState, useCallback } from 'react'
import type { RecommendationItem } from '../../types/api'
import { api } from '../../api/client'
import { ApiError } from '../../api/errors'
import { normalizeUsername } from '../../utils/normalize'

type UseRandomRecommendationsResult = {
  isLoading: boolean
  error: string | null
  items: RecommendationItem[]
  load: (username: string, count?: number) => Promise<void>
}

export function useRandomRecommendations(): UseRandomRecommendationsResult {
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [items, setItems] = useState<RecommendationItem[]>([])

  const load = useCallback(async (username: string, count = 3): Promise<void> => {
    const normalizedUsername = normalizeUsername(username)
    if (!normalizedUsername) {
      setError('Username is required.')
      setItems([])
      return
    }
    const itemCount = count > 0 ? count : 3
    setIsLoading(true)
    setError(null)
    try {
      const response = await api.recommendRandom(normalizedUsername, itemCount)
      setItems(response)
    } catch (error: unknown) {
      if (error instanceof ApiError) {
        setError(error.message)
      } else if (error instanceof Error) {
        setError(error.message)
      } else {
        setError('Unexpected error occurred while loading random recommendations.')
      }
    } finally {
      setIsLoading(false)
    }
  }, [])

  return {
    isLoading,
    error,
    items,
    load,
  }
}
