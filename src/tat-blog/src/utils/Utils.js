import { useMemo } from 'react'
import { useLocation } from 'react-router-dom'

export function isEmptyOrSpaces(str) {
  return str == null || (typeof str === 'string' && str.match(/^ *$/) !== null)
}

export const useQuery = () => {
  const { search } = useLocation()
  return useMemo(() => new URLSearchParams(search), [search])
}
