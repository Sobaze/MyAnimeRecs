type ImportFormProps = {
  username: string
  onUsernameChange: (value: string) => void
  onSubmit: () => void
  disabled?: boolean
}

export function ImportForm({ username, onUsernameChange, onSubmit, disabled = false }: ImportFormProps) {
  return (
    <div>
      <input
        value={username}
        onChange={(event) => onUsernameChange(event.target.value)}
        placeholder="MAL username"
      />
      <button type="button" onClick={onSubmit} disabled={disabled}>
        Import
      </button>
    </div>
  )
}
