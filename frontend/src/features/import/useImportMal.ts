import { useState } from 'react'
import type { ImportSummary } from '../../types/api'

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
    void username
    setIsLoading(true)
    setError(null)
    setSummary(null)
    setIsLoading(false)
    throw new Error('Not implemented yet: useImportMal.runImport')
  }

  return {
    isLoading,
    error,
    summary,
    runImport,
  }
}
