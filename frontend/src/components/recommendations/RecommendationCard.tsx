import type { RecommendationItem } from '../../types/api'

type RecommendationCardProps = {
  item: RecommendationItem
}

export function RecommendationCard({ item }: RecommendationCardProps) {
  const animeUrl = `https://myanimelist.net/anime/${item.sourceAnimeId}`
  return (
    <article className="anime-card">
      <h3 className="card-title">
        <a href={animeUrl} target="_blank" rel="noopener noreferrer">
          {item.title}
        </a>
      </h3>
      <div className="card-content">
        {item.mainPictureMediumUrl && (
          <img
            className="card-image"
            src={item.mainPictureMediumUrl}
            alt={item.title}
            loading="lazy"
          />
        )}
        <p className="card-summary">{item.synopsis ?? item.reason}</p>
      </div>
      <footer className="card-score-row">
        <span>MAL avg: {item.meanScore ?? 'N/A'}</span>
        <span>Match score: {item.score.toFixed(2)}</span>
      </footer>
    </article>
  )
}
