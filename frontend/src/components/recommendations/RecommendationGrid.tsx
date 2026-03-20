import type { RecommendationItem } from '../../types/api'
import { RecommendationCard } from './RecommendationCard'

type RecommendationGridProps = {
  items: RecommendationItem[]
}

export function RecommendationGrid({ items }: RecommendationGridProps) {
  return (
    <div className="anime-grid">
      {items.map((item) => (
        <RecommendationCard key={item.animeId} item={item} />
      ))}
    </div>
  )
}
