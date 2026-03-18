import type { UserAnimeListItem } from '../../types/api'

type AnimeCardProps = {
  anime: UserAnimeListItem
}

export function AnimeCard({ anime }: AnimeCardProps) {
  return (
    <article>
      <h3>{anime.title}</h3>
      <p>User Score: {anime.userScore ?? 'N/A'}</p>
      {anime.mainPictureMediumUrl && <img src={anime.mainPictureMediumUrl} alt={anime.title} style={{ width: '100%', display: 'block' }} />}
    </article>
  )
}
