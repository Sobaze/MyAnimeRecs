import { apiEndpoints } from './endpoints'
import type {
  ImportSummary,
  RecommendForFriendRequest,
  RecommendationItem,
  TopGenre,
  UserAnimeListItem,
} from '../types/api'

export async function importMalUser(username: string): Promise<ImportSummary> {
  void username
  throw new Error('Not implemented yet: importMalUser')
}

export async function getUserAnimeList(
  username: string,
  status: 'completed' | 'watching' | 'on_hold' | 'dropped' | 'plan_to_watch' = 'completed',
): Promise<UserAnimeListItem[]> {
  void username
  void status
  throw new Error('Not implemented yet: getUserAnimeList')
}

export async function getUserTopGenres(username: string, limit = 10): Promise<TopGenre[]> {
  void username
  void limit
  throw new Error('Not implemented yet: getUserTopGenres')
}

export async function recommendForFriend(
  username: string,
  request: RecommendForFriendRequest,
): Promise<RecommendationItem[]> {
  void username
  void request
  throw new Error('Not implemented yet: recommendForFriend')
}

export async function recommendRandom(username: string, count = 3): Promise<RecommendationItem[]> {
  void username
  void count
  throw new Error('Not implemented yet: recommendRandom')
}

export async function parseJsonResponse<T>(response: Response): Promise<T> {
  void response
  throw new Error('Not implemented yet: parseJsonResponse')
}

export const api = {
  importMalUser,
  getUserAnimeList,
  getUserTopGenres,
  recommendForFriend,
  recommendRandom,
  parseJsonResponse,
  endpoints: apiEndpoints,
}
