import { useState } from 'react'
import type { TopGenre } from '../../types/api'
import { api } from '../../api/client'
import { ApiError } from '../../api/errors'
import { normalizeUsername } from '../../utils/normalize'

type UseTopGenresResult = {
  isLoading: boolean
  error: string | null
  items: TopGenre[]
  load: (username: string, limit?: number) => Promise<void>
}

export function useTopGenres(): UseTopGenresResult {
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [items, setItems] = useState<TopGenre[]>([])

  async function load(username: string, limit = 10): Promise<void> {
    const normalizedUsername = normalizeUsername(username)
    if (!normalizedUsername) {
      setError('Username is required.')
      setItems([])
      return
    }
    const limitValue = limit > 0 ? limit : 10
    setIsLoading(true)
    try {
      const response = await api.getUserTopGenres(normalizedUsername, limitValue)
      setItems(response)
    } catch (error: unknown) {
      if (error instanceof ApiError) {
        setError(error.message)
      } else if (error instanceof Error) {
        setError(error.message)
      } else {
        setError('Unexpected error occurred while loading top genres.')
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
