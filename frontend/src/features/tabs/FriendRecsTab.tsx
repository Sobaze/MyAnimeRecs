import { useEffect, useState } from 'react'
import { useTopGenres } from '../anime-list/useTopGenres'
import { useFriendRecommendations } from '../recommendations/useFriendRecommendations'
import { RecommendationGrid } from '../../components/recommendations/RecommendationGrid'
import { LoadingState } from '../../components/common/LoadingState'
import { ErrorState } from '../../components/common/ErrorState'
import { EmptyState } from '../../components/common/EmptyState'

type FriendRecsTabProps = {
  username: string
}

export function FriendRecsTab({ username }: FriendRecsTabProps) {
  const { items: genres, isLoading: genresLoading, error: genresError, load: loadGenres } = useTopGenres()
  const { isLoading: recsLoading, error: recsError, items: recs, load: loadRecs } = useFriendRecommendations()
  const [selectedGenres, setSelectedGenres] = useState<string[]>([])
  const [hasRequestedRecs, setHasRequestedRecs] = useState(false)
  const [maxItems, setMaxItems] = useState(12)
  const [lastRequestedGenresKey, setLastRequestedGenresKey] = useState<string | null>(null)

  const selectedGenresKey = [...selectedGenres].sort((a, b) => a.localeCompare(b)).join('|')

  useEffect(() => {
    loadGenres(username)
  }, [username, loadGenres])


  function toggleGenre(name: string) {
    setSelectedGenres(prev =>
      prev.includes(name) ? prev.filter(g => g !== name) : [...prev, name]
    )
  }

  function handleSubmit() {
    if (selectedGenres.length === 0) return

    setHasRequestedRecs(true)
    setMaxItems(12)
    setLastRequestedGenresKey(selectedGenresKey)
    loadRecs(username, selectedGenres, 12)
  }

  function handleRequestMore() {
    const newMaxItems = Math.min(maxItems + 12, 50)
    setMaxItems(newMaxItems)
    loadRecs(username, selectedGenres, newMaxItems)
  }

  const canLoadMore = hasRequestedRecs
    && !recsLoading
    && !recsError
    && recs.length > 0
    && selectedGenres.length > 0
    && selectedGenresKey === lastRequestedGenresKey
    && maxItems < 50

  return (
    <div>
      {genresLoading && <LoadingState text="Loading your genres..." />}
      {genresError && <ErrorState message={genresError} />}
      {!genresLoading && genres.length > 0 && (
        <div className='genre-grid'>
          {genres.map((genre) => {
            const isSelected = selectedGenres.includes(genre.name)
            return (
              <label key={genre.name} className={`genre-options ${isSelected ? 'is-selected' : ''} `}>
                <input
                  type="checkbox"
                  checked={isSelected}
                  onChange={() => toggleGenre(genre.name)}
                />
                {genre.name}
              </label>
            )
          })}
        </div>
      )}
      <button
        type="button"
        onClick={handleSubmit}
        disabled={recsLoading || selectedGenres.length === 0}
      >
        Get recommendations
      </button>
      {recsLoading && <LoadingState text="Finding recommendations for your friend..." />}
      {recsError && <ErrorState message={recsError} />}
      {hasRequestedRecs && !recsLoading && !recsError && recs.length === 0 && selectedGenres.length > 0 && (
        <EmptyState text="No recommendations found for those genres." />
      )}
      {recs.length > 0 && (
        <RecommendationGrid items={recs} />
      )}
      {canLoadMore && (
        <button
          type="button"
          onClick={handleRequestMore}
          disabled={recsLoading || selectedGenres.length === 0}
        >
          Load more recommendations
        </button>
      )}
    </div>
  )
}
