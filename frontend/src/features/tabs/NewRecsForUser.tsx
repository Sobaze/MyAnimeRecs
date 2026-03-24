import { useEffect } from "react";
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

  useEffect(() => {
    load(username);
  }, [username, load]);

  return (
    <div>
      {isLoading && <LoadingState text="Finding recommendations..." />}
      {error && <ErrorState message={error} />}
        {!isLoading && !error && items.length === 0 && (
          <EmptyState text="No new recommendations available." />
        )}
        {!isLoading && !error && items.length > 0 && (
          <RecommendationGrid items={items} />
        )}
    </div>
  )
}