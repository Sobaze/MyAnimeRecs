import {useState } from 'react'
import './App.css'
import { useImportMal } from './features/import/useImportMal'
import { AppShell } from './components/layout/AppShell'
import { ImportForm } from './components/import/ImportForm'
import { ImportSummary } from './components/import/ImportSummary'
import { LoadingState } from './components/common/LoadingState'
import { ErrorState } from './components/common/ErrorState'
import { MyListTab } from './features/tabs/MyListTab'
import { RandomTab } from './features/tabs/RandomTab'
import { FriendRecsTab } from './features/tabs/FriendRecsTab'

type Tab = 'random' | 'friend' | 'list'

function App() {
  const [inputUsername, setInputUsername] = useState('')
  const [activeTab, setActiveTab] = useState<Tab>('list')

  const { isLoading, error, summary, runImport } = useImportMal()
  const confirmedUsername = summary?.username ?? ''

  function handleImport() {
    runImport(inputUsername)
  }

  if (!confirmedUsername) {
    return (
      <AppShell>
        <ImportForm
          username={inputUsername}
          onUsernameChange={setInputUsername}
          onSubmit={handleImport}
          disabled={isLoading}
        />
        {isLoading && <LoadingState text="Importing your anime list..." />}
        {error && <ErrorState message={error} />}
        {summary && <ImportSummary summary={summary} />}
      </AppShell>
    )
  }

  return (
    <AppShell>
      <nav>
        <button
          type="button"
          onClick={() => setActiveTab('list')}
          disabled={activeTab === 'list'}
        >
          My List
        </button>
        <button
          type="button"
          onClick={() => setActiveTab('random')}
          disabled={activeTab === 'random'}
        >
          Random
        </button>
        <button
          type="button"
          onClick={() => setActiveTab('friend')}
          disabled={activeTab === 'friend'}
        >
          For a Friend
        </button>
      </nav>
      {activeTab === 'list' && <MyListTab username={confirmedUsername} />}
      {activeTab === 'random' && <RandomTab username={confirmedUsername} />}
      {activeTab === 'friend' && <FriendRecsTab key={confirmedUsername} username={confirmedUsername} />}
    </AppShell>
  )
}

export default App
