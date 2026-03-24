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

  useEffect(() => {
    loadGenres(username)
  }, [username, loadGenres])


  function toggleGenre(name: string) {
    setSelectedGenres(prev =>
      prev.includes(name) ? prev.filter(g => g !== name) : [...prev, name]
    )
  }

  function handleSubmit() {
    setHasRequestedRecs(true)
    loadRecs(username, selectedGenres)
  }

  return (
    <div>
      {genresLoading && <LoadingState text="Loading your genres..." />}
      {genresError && <ErrorState message={genresError} />}
      {!genresLoading && genres.length > 0 && (
        <div className='genre-grid'>
          {genres.map(genre => (
            <label key={genre.name} className='genre-options'>
              <input
                type="checkbox"
                checked={selectedGenres.includes(genre.name)}
                onChange={() => toggleGenre(genre.name)}
              />
              {genre.name}
            </label>
          ))}
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
      {!recsLoading && recs.length > 0 && (
        <RecommendationGrid items={recs} />
      )}
    </div>
  )
}
