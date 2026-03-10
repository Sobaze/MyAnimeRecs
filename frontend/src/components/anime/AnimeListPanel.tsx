import type { UserAnimeListItem } from '../../types/api'
import { AnimeGrid } from './AnimeGrid'

type AnimeListPanelProps = {
  items: UserAnimeListItem[]
}

export function AnimeListPanel({ items }: AnimeListPanelProps) {
  return <AnimeGrid items={items} />
}
