import { useEffect } from 'react'
import { useRandomRecommendations } from '../recommendations/useRandomRecommendations'
import { RecommendationGrid } from '../../components/recommendations/RecommendationGrid'
import { LoadingState } from '../../components/common/LoadingState'
import { ErrorState } from '../../components/common/ErrorState'
import { EmptyState } from '../../components/common/EmptyState'

type RandomTabProps = {
  username: string
}

export function RandomTab({ username }: RandomTabProps) {
  const { isLoading, error, items, load } = useRandomRecommendations()

  useEffect(() => {
    load(username)
  }, [username, load])

  function handleReroll() {
    load(username)
  }

  return (
    <div className="tab-panel">
      <div className="tab-controls">
        <button type="button" onClick={handleReroll} disabled={isLoading}>
          Roll new recommendations
        </button>
      </div>
      {isLoading && <LoadingState text="Finding recommendations..." />}
      {error && <ErrorState message={error} />}
      {!isLoading && !error && items.length === 0 && (
        <EmptyState text="No recommendations found." />
      )}
      {!isLoading && !error && items.length > 0 && (
        <RecommendationGrid items={items} />
      )}
    </div>
  )
}
