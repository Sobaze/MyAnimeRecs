import type { UserAnimeListItem } from '../../types/api'

type AnimeCardProps = {
  anime: UserAnimeListItem
}
export function AnimeCard({ anime }: AnimeCardProps) {
  const animeUrl = `https://myanimelist.net/anime/${anime.sourceAnimeId}`
  return (
    <article className="anime-card">
      <h3 className="card-title">
        <a href={animeUrl} target="_blank" rel="noopener noreferrer">
          {anime.title}
        </a>
      </h3>
      <div className="card-content">
        {anime.mainPictureMediumUrl && (
          <img
            className="card-image"
            src={anime.mainPictureMediumUrl}
            alt={anime.title}
            loading="lazy"
          />
        )}
        <p className="card-summary">{anime.synopsis ?? 'No synopsis available.'}</p>
      </div>
      <footer className="card-score-row">
        <span>User score: {anime.userScore ?? 'N/A'}</span>
        <span>MAL score: {anime.meanScore ?? 'N/A'}</span>
      </footer>
    </article>
  )
}
