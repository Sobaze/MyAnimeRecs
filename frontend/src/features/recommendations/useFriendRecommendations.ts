import { useState } from 'react'
import type { RecommendationItem } from '../../types/api'

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
    void username
    void genres
    void maxItems
    setIsLoading(true)
    setError(null)
    setItems([])
    setIsLoading(false)
    throw new Error('Not implemented yet: useFriendRecommendations.load')
  }

  return {
    isLoading,
    error,
    items,
    load,
  }
}
