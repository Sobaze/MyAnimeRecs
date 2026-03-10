import type { RecommendationItem } from '../../types/api'
import { RecommendationGrid } from './RecommendationGrid'

type RandomRecommendationPanelProps = {
  items: RecommendationItem[]
}

export function RandomRecommendationPanel({ items }: RandomRecommendationPanelProps) {
  return <RecommendationGrid items={items} />
}
