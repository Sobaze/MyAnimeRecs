import type { UserAnimeListItem } from '../../types/api'
import { AnimeCard } from './AnimeCard'

type AnimeGridProps = {
  items: UserAnimeListItem[]
}

export function AnimeGrid({ items }: AnimeGridProps) {
  return (
    <div>
      {items.map((anime) => (
        <AnimeCard key={anime.animeId} anime={anime} />
      ))}
    </div>
  )
}
