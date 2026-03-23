import { useEffect } from 'react'
import { useAnimeList } from '../anime-list/useAnimeList'
import { AnimeGrid } from '../../components/anime/AnimeGrid'
import { LoadingState } from '../../components/common/LoadingState'
import { ErrorState } from '../../components/common/ErrorState'
import { EmptyState } from '../../components/common/EmptyState'

type MyListTabProps = {
  username: string
}

export function MyListTab({ username }: MyListTabProps) {
  const { isLoading, error, items, load } = useAnimeList()
  useEffect(() => {
    load(username)
  }, [username, load])

  if (isLoading) return <LoadingState text="Loading your anime list..." />
  if (error) return <ErrorState message={error} />
  if (items.length === 0) return <EmptyState text="No anime found in your completed list." />

  return (

  <AnimeGrid items={items} />

)
}
