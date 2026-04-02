import { useEffect, useState } from 'react'
import { useAnimeList } from '../anime-list/useAnimeList'
import { AnimeGrid } from '../../components/anime/AnimeGrid'
import { LoadingState } from '../../components/common/LoadingState'
import { ErrorState } from '../../components/common/ErrorState'
import { EmptyState } from '../../components/common/EmptyState'

type MyListTabProps = {
  username: string
}
type SortOrder = 'score' | 'title'  | 'mean score'

export function MyListTab({ username }: MyListTabProps) {
  const { isLoading, error, items, load } = useAnimeList()
  const [sortOrder, setSortOrder] = useState<SortOrder>('title')
  useEffect(() => {
    load(username)
  }, [username, load])

  const sortedItems = [...items]
  if (sortOrder === 'score') {
    sortedItems.sort((a, b) => {
    const scoreDiff = (b.userScore ?? 0) - (a.userScore ?? 0) // primary: score desc
    if (scoreDiff !== 0) {
      return scoreDiff
    }
    return a.title.localeCompare(b.title) // secondary: title asc
  })
  } else if (sortOrder === 'title') {
    sortedItems.sort((a, b) => a.title.localeCompare(b.title))
  } else if (sortOrder === 'mean score') {
    
    sortedItems.sort((a, b) => {
      const meanScoreDiff = (b.meanScore || 0) - (a.meanScore || 0)
      if (meanScoreDiff !== 0) { 
        return meanScoreDiff
      }
      return a.title.localeCompare(b.title) // secondary: title asc
    })
  }
 
  return (
    <div className="tab-panel">
      <div className="tab-controls">
        <label className='sort-control' >
          <span>Sort by:</span>
          <select value={sortOrder} 
          onChange={(e) => setSortOrder(e.target.value as SortOrder)}>
            <option value="title">Sort by Title</option>
            <option value="score">Sort by Your Score</option>
            <option value="mean score">Sort by MAL Score</option>
          </select>
        </label>
      </div>

      {isLoading && <LoadingState text="Loading your anime list..." />}
      {error && <ErrorState message={error} />}
      {!isLoading && !error && items.length === 0 && <EmptyState text="No anime found in your completed list." />}


      { !isLoading && !error && items.length > 0 && <AnimeGrid items={sortedItems} /> }
    </div>
  )
}
