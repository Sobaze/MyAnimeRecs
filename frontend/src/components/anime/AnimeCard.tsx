import type { UserAnimeListItem } from '../../types/api'

type AnimeCardProps = {
  anime: UserAnimeListItem
}

export function AnimeCard({ anime }: AnimeCardProps) {
  return (
    <article>
      <img src={anime.mainPictureMediumUrl ?? ''} alt={anime.title} />
      <h3>{anime.title}</h3>
    </article>
  )
}
