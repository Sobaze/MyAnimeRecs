import { useState, useCallback } from "react";
import type { RecommendationItem } from "../../types/api";
import { api } from "../../api/client";
import { ApiError } from "../../api/errors";
import { normalizeUsername } from "../../utils/normalize";

type UseNewRecommendationsForUserResult = {
  isLoading: boolean;
  error: string | null;
  items: RecommendationItem[];
  load: (username: string, maxItems?: number) => Promise<void>;
};

export function useNewRecommendationsForUser(): UseNewRecommendationsForUserResult {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [items, setItems] = useState<RecommendationItem[]>([])

    const load = useCallback(async (username: string, maxItems?: number): Promise<void> => {
    const normalizedUsername = normalizeUsername(username);
    if (!normalizedUsername) {
      setError("Username is required.");
      setItems([]);
      return;
    }
    setIsLoading(true);
    setError(null);
    maxItems = maxItems && maxItems > 0 ? maxItems : 12
    try {
      const response = await api.recommendNewForUser(normalizedUsername, maxItems);
      setItems(response);
    } catch (error: unknown) {
      if (error instanceof ApiError) {
        setError(error.message);
      } else if (error instanceof Error) {
        setError(error.message);
      } else {
        setError("Unexpected error occurred while loading recommendations.");
      } 
    } finally {
        setIsLoading(false);
      }
    }, []);

  return {
    isLoading,
    error,
    items,
    load,
  }
}