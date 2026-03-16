import type { RecommendationItem } from '../../types/api'

type RecommendationCardProps = {
  item: RecommendationItem
}

export function RecommendationCard({ item }: RecommendationCardProps) {
  return (
    <article>
      <h3>{item.title}</h3>
      <p>{item.reason}</p>
      <img src={item.mainPictureMediumUrl ?? ''} alt={item.title} />
    </article>
  )
}
