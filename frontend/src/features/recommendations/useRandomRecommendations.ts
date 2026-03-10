import { useState } from 'react'
import type { RecommendationItem } from '../../types/api'

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

  async function load(username: string, count = 3): Promise<void> {
    void username
    void count
    setIsLoading(true)
    setError(null)
    setItems([])
    setIsLoading(false)
    throw new Error('Not implemented yet: useRandomRecommendations.load')
  }

  return {
    isLoading,
    error,
    items,
    load,
  }
}
