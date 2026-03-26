import { useEffect, useState } from "react";
import { useNewRecommendationsForUser } from "../recommendations/useNewRecByMALUsername";
import { RecommendationGrid } from "../../components/recommendations/RecommendationGrid";
import { LoadingState } from "../../components/common/LoadingState";
import { ErrorState } from "../../components/common/ErrorState";
import { EmptyState } from "../../components/common/EmptyState";

type NewRecsForUserProps = {
  username: string;
};
export function NewRecsForUser({ username }: NewRecsForUserProps) {
  const { isLoading, error, items, load } = useNewRecommendationsForUser();
  const [getMoreRec, setGetMoreRec] = useState(12)

  useEffect(() => {
    load(username, 12);
  }, [username, load]);
  function handleRequestMore() {
    const newGetMoreRec = Math.min(getMoreRec + 12, 50)
    setGetMoreRec(newGetMoreRec)
    load(username, newGetMoreRec);
  }

  return (
    <div>
      
        {isLoading && <LoadingState text="Finding recommendations..." />}
        {error && <ErrorState message={error} />}
        {!isLoading && !error && items.length === 0 && (
          <EmptyState text="No new recommendations available." />
        )}
        {!error && items.length > 0 && (
          <RecommendationGrid items={items} />
        )}
        <button
          type="button"
          onClick={handleRequestMore}
        >
          Load more recommendations
        </button>
      
    </div>
  )
}