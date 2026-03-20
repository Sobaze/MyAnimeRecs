import { useCallback, useState } from 'react'
import { api } from '../../api/client'
import { ApiError } from '../../api/errors'
import { normalizeUsername } from '../../utils/normalize'

type Step = 'idle' | 'catalog' | 'import' | 'done'

type UseImportFlowResult = {
  step: Step
  error: string | null
  confirmedUsername: string
  startImport: (username: string) => Promise<void>
  reset: () => void
}

export function useImportFlow(): UseImportFlowResult {
  const [step, setStep] = useState<Step>('idle')
  const [error, setError] = useState<string | null>(null)
  const [confirmedUsername, setConfirmedUsername] = useState('')

  const startImport = useCallback(async (username: string): Promise<void> => {
    const normalizedUsername = normalizeUsername(username)
    if (!normalizedUsername) {
      setError('Username is required.')
      return
    }
    setError(null)
    setStep('catalog')
    try {
        await api.ensureCatalogFresh()
        setStep('import')
        const result = await api.importMalUser(normalizedUsername)
        setConfirmedUsername(result.username)
        setStep('done')
        
    } catch (err) {
        setStep('idle')
        if (err instanceof ApiError) {
            setError(err.message)
        } else if (err instanceof Error) {
            setError(err.message)
        } else {
            setError('An unexpected error occurred.')
        }
    }
  }, [])

  const reset = useCallback(() => {
    setStep('idle')
    setError(null)
    setConfirmedUsername('')
  }, [])

  return {
    step,
    error,
    confirmedUsername,
    startImport,
    reset,
  }
}
