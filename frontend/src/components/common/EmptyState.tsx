type EmptyStateProps = {
  text: string
}

export function EmptyState({ text }: EmptyStateProps) {
  return <p>{text}</p>
}
