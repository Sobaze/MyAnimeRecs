export type LoadState = 'idle' | 'loading' | 'success' | 'error'

export type AsyncState<T> = {
  state: LoadState
  data: T | null
  error: string | null
}
