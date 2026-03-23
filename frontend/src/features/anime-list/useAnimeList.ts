import { useState, useCallback } from 'react'
import type { UserAnimeListItem } from '../../types/api'
import { api } from '../../api/client'
import { ApiError } from '../../api/errors'
import { normalizeUsername } from '../../utils/normalize'

type UseAnimeListResult = {
  isLoading: boolean
  error: string | null
  items: UserAnimeListItem[]
  load: (username: string) => Promise<void>
}

export function useAnimeList(): UseAnimeListResult {
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [items, setItems] = useState<UserAnimeListItem[]>([])

  const load = useCallback(async (username: string): Promise<void> => {
    const normalizedUsername = normalizeUsername(username)
    if (!normalizedUsername) {
      setError('Username is required.')
      setItems([])
      return
    }
    setIsLoading(true)
    setError(null)
    try {
      const response = await api.getUserAnimeList(normalizedUsername, 'completed')
      setItems(response)
    } catch (error: unknown) {
      if (error instanceof ApiError) {
        setError(error.message)
      } else if (error instanceof Error) {
        setError(error.message)
      } else {
        setError('Unexpected error occurred while loading anime list.')
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
