import { useState } from 'react'
import type { ImportSummary } from '../../types/api'
import { api } from '../../api/client'
import { ApiError } from '../../api/errors'
import { normalizeUsername } from '../../utils/normalize'

type UseImportMalResult = {
  isLoading: boolean
  error: string | null
  summary: ImportSummary | null
  runImport: (username: string) => Promise<void>
}

export function useImportMal(): UseImportMalResult {
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [summary, setSummary] = useState<ImportSummary | null>(null)

  async function runImport(username: string): Promise<void> {
    const normalizedUsername = normalizeUsername(username)
    if (!normalizedUsername) {
      setError('Username is required.')
      setSummary(null)
      return
    }

    setIsLoading(true)
    try {
      const result = await api.importMalUser(normalizedUsername)
      setSummary(result)
    } catch (error: unknown) {
      setSummary(null)
      if (error instanceof ApiError) {
        setError(error.message)
      } else if (error instanceof Error) {
        setError(error.message)
      } else {
        setError('Unexpected error occurred while importing MAL list.')
      }
    } finally {
      setIsLoading(false)
    }
  }

  return {
    isLoading,
    error,
    summary,
    runImport,
  }
}
