import { useState } from 'react'
import type { UserAnimeListItem } from '../../types/api'

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

  async function load(username: string): Promise<void> {
    void username
    setIsLoading(true)
    setError(null)
    setItems([])
    setIsLoading(false)
    throw new Error('Not implemented yet: useAnimeList.load')
  }

  return {
    isLoading,
    error,
    items,
    load,
  }
}
