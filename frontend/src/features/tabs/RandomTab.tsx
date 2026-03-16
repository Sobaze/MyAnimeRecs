import { useEffect } from 'react'
import { useRandomRecommendations } from '../recommendations/useRandomRecommendations'
import { RandomRecommendationPanel } from '../../components/recommendations/RandomRecommendationPanel'
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
    <div>
      <button type="button" onClick={handleReroll} disabled={isLoading}>
        Roll new recommendations
      </button>
      {isLoading && <LoadingState text="Finding recommendations..." />}
      {error && <ErrorState message={error} />}
      {!isLoading && !error && items.length === 0 && (
        <EmptyState text="No recommendations found." />
      )}
      {!isLoading && !error && items.length > 0 && (
        <RandomRecommendationPanel items={items} />
      )}
    </div>
  )
}
