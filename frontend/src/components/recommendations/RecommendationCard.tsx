import type { RecommendationItem } from '../../types/api'

type RecommendationCardProps = {
  item: RecommendationItem
}

export function RecommendationCard({ item }: RecommendationCardProps) {
  return (
    <article>
      <img src={item.mainPictureMediumUrl ?? ''} alt={item.title} />
      <h3>{item.title}</h3>
      <p>{item.reason}</p>
    </article>
  )
}
