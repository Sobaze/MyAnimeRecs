export const apiEndpoints = {
  importMalUser: (username: string) => `/api/import/mal/${encodeURIComponent(username)}`,

  getUserAnimeList: (username: string, status = 'completed') =>
    `/api/users/${encodeURIComponent(username)}/anime-list?status=${encodeURIComponent(status)}`,

  getUserTopGenres: (username: string, limit = 10) =>
    `/api/users/${encodeURIComponent(username)}/genres/top?limit=${limit}`,

  recommendForFriend: (username: string) =>
    `/api/recommendations/for-friend/${encodeURIComponent(username)}`,

  recommendRandom: (username: string, count = 3) =>
    `/api/recommendations/random/${encodeURIComponent(username)}?count=${count}`,
}
