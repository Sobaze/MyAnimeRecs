import { useState } from 'react'
import './App.css'
import { useImportFlow } from './features/import/useImportFlow'
import { AppShell } from './components/layout/AppShell'
import { ImportForm } from './components/import/ImportForm'
import { LoadingState } from './components/common/LoadingState'
import { ErrorState } from './components/common/ErrorState'
import { MyListTab } from './features/tabs/MyListTab'
import { RandomTab } from './features/tabs/RandomTab'
import { FriendRecsTab } from './features/tabs/FriendRecsTab'

type Tab = 'random' | 'friend' | 'list'

function App() {
  const [inputUsername, setInputUsername] = useState('')
  const [activeTab, setActiveTab] = useState<Tab>('list')

  const { step, error, confirmedUsername, startImport, reset } = useImportFlow()

  function handleImport() {
    startImport(inputUsername)
  }

  function handleChangeUser() {
    setInputUsername('')
    setActiveTab('list')
    reset()
  }

  if (!confirmedUsername) {
    return (
      <AppShell>
        <ImportForm
          username={inputUsername}
          onUsernameChange={setInputUsername}
          onSubmit={handleImport}
          disabled={step === 'catalog' || step === 'import'}
        />
        {step === 'catalog' && <LoadingState text="Preparing anime catalog..." />}
        {step === 'import' && <LoadingState text="Importing your MAL list..." />}
        {error && <ErrorState message={error} />}
      </AppShell>
    )
  }

  return (
      <AppShell>
        <div className="topbar">
          <p className="identity">Current user: <strong>{confirmedUsername}</strong></p>
          <button type="button" className="secondary" onClick={handleChangeUser}>
            Change user
          </button>
        </div>
        <nav className="tabs-nav">
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
        <div hidden={activeTab !== 'list'}>
          <MyListTab username={confirmedUsername} />
        </div>
        <div hidden={activeTab !== 'random'}>
          <RandomTab username={confirmedUsername} />
        </div>
      <div hidden={activeTab !== 'friend'}>
        <FriendRecsTab username={confirmedUsername} />
      </div>
    </AppShell>
  )
}

export default App
