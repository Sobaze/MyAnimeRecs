import type { UserAnimeListItem } from '../../types/api'

type AnimeCardProps = {
  anime: UserAnimeListItem
}
///${anime.title.replace(/\s+/g, '_') eventually add this for cleaner urls, but for now just link to the anime page on MAL using the sourceAnimeId
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
        <span>MAL avg: {anime.meanScore ?? 'N/A'}</span>
      </footer>
    </article>
  )
}
