type LoadingStateProps = {
  text?: string
}

export function LoadingState({ text = 'Loading...' }: LoadingStateProps) {
  return <p>{text}</p>
}
