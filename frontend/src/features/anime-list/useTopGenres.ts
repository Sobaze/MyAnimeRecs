import { useState } from 'react'
import type { TopGenre } from '../../types/api'

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
    void username
    void limit
    setIsLoading(true)
    setError(null)
    setItems([])
    setIsLoading(false)
    throw new Error('Not implemented yet: useTopGenres.load')
  }

  return {
    isLoading,
    error,
    items,
    load,
  }
}
