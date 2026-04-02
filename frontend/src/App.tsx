import { useEffect, useState } from 'react'
import './App.css'
import { useImportFlow } from './features/import/useImportFlow'
import { AppShell } from './components/layout/AppShell'
import { ImportForm } from './components/import/ImportForm'
import { LoadingState } from './components/common/LoadingState'
import { ErrorState } from './components/common/ErrorState'
import { MyListTab } from './features/tabs/MyListTab'
import { RandomTab } from './features/tabs/RandomTab'
import { FriendRecsTab } from './features/tabs/FriendRecsTab'
import { NewRecsForUser } from './features/tabs/NewRecsForUser'

type Tab = 'random' | 'friend' | 'list' | 'forUser'

function App() {
  const [inputUsername, setInputUsername] = useState('')
  const [activeTab, setActiveTab] = useState<Tab>('list')
  const [showScrollTop, setShowScrollTop] = useState(false)

  const { step, error, confirmedUsername, startImport, reset } = useImportFlow()

  const TAB_OPTIONS: {key: Tab, label: string}[] = [
    { key: 'list', label: 'My Completed List' },
    { key: 'forUser', label: 'New Recommendations' },
    { key: 'random', label: 'Random Recommendations' },
    { key: 'friend', label: 'Recommendations for Friend' },
  ]
  useEffect(() => {
    function handleScroll() {
      setShowScrollTop(window.scrollY > 300)
    }
    window.addEventListener('scroll', handleScroll)
    return () => window.removeEventListener('scroll', handleScroll)
  }, [])

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
        <div className='import-form' >
          <ImportForm
            username={inputUsername}
            onUsernameChange={setInputUsername}
            onSubmit={handleImport}
            disabled={step === 'catalog' || step === 'import'}
          />
        </div>
        {step === 'catalog' && <LoadingState text="Preparing anime catalog..." />}
        {step === 'import' && <LoadingState text="Importing your MAL list..." />}
        {error && <ErrorState message={error} />}
      </AppShell>
    )
  }

  return (
      <AppShell>
        <div className="topbar">
          <p className="identity">MAL users list: <strong>{confirmedUsername}</strong></p>
          <button type="button" className="secondary" onClick={handleChangeUser}>
            Change user
          </button>
        </div>
        <nav className="tabs-nav">
          {TAB_OPTIONS.map((option => (
            <button
              key={option.key}
              type="button"
              onClick={() => setActiveTab(option.key)}
              className={activeTab === option.key ? 'is-active' : ''}
            >
              {option.label}
            </button>
          )))}   
        
        </nav>
        <div hidden={activeTab !== 'list'}>
          <MyListTab username={confirmedUsername} />
        </div>
        <div hidden={activeTab !== 'forUser'}>
          <NewRecsForUser key={confirmedUsername} username={confirmedUsername} />
        </div>
        <div hidden={activeTab !== 'random'}>
          <RandomTab username={confirmedUsername} />
        </div>
      <div hidden={activeTab !== 'friend'}>
        <FriendRecsTab username={confirmedUsername} />
      </div>
      <div>
        {showScrollTop && (
          <button
            className="scroll-top-btn"
            type='button'
            aria-label='Scroll to top'
            onClick={() => window.scrollTo({ top: 0, behavior: 'smooth' })}
          >
            ↑
          </button>
        )}
      </div>
    </AppShell>
  )
}

export default App
