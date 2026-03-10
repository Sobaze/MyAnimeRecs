type FriendRecommendationFormProps = {
  genresInput: string
  onGenresInputChange: (value: string) => void
  onSubmit: () => void
  disabled?: boolean
}

export function FriendRecommendationForm({
  genresInput,
  onGenresInputChange,
  onSubmit,
  disabled = false,
}: FriendRecommendationFormProps) {
  return (
    <div>
      <input
        value={genresInput}
        onChange={(event) => onGenresInputChange(event.target.value)}
        placeholder="Action, Drama"
      />
      <button type="button" onClick={onSubmit} disabled={disabled}>
        Recommend
      </button>
    </div>
  )
}
