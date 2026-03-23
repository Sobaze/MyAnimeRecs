export const apiEndpoints = {
  getCatalogStatus: () => '/api/catalog/status',
  ensureCatalogFresh: () => '/api/catalog/ensure-fresh',

  importMalUser: (username: string) => `/api/import/mal/${encodeURIComponent(username)}`,

  getUserAnimeList: (username: string, status: string) =>
    `/api/users/${encodeURIComponent(username)}/anime-list?status=${encodeURIComponent(status)}`,

  getUserTopGenres: (username: string, limit = 10) =>
    `/api/users/${encodeURIComponent(username)}/genres/top?limit=${limit}`,

  recommendForFriend: (username: string) =>
    `/api/recommendations/for-friend/${encodeURIComponent(username)}`,

  recommendRandom: (username: string, count = 3) =>
    `/api/recommendations/random/${encodeURIComponent(username)}?count=${count}`,
}
