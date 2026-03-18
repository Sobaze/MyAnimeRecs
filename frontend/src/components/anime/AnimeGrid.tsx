import type { UserAnimeListItem } from '../../types/api'
import { AnimeCard } from './AnimeCard'

type AnimeGridProps = {
  items: UserAnimeListItem[]
}

export function AnimeGrid({ items }: AnimeGridProps) {
  return (
    <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(160px, 1fr))', gap: '1rem' }}>
      {items.map((anime) => (
        <AnimeCard key={anime.animeId} anime={anime}  />
      ))}
    </div>
  )
}
