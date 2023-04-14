import { useMemo } from 'react'
import { useLocation } from 'react-router-dom'

export function isEmptyOrSpaces(str) {
  return str == null || (typeof str === 'string' && str.match(/^ *$/) !== null)
}

export const useQuery = () => {
  const { search } = useLocation()
  return useMemo(() => new URLSearchParams(search), [search])
}

export function isInteger(str) {
  return Number.isInteger(Number(str)) && Number(str) >= 0
}

export function decode(str) {
  const txt = new DOMParser().parseFromString(str, 'text/html')
  return txt.documentElement.textContent
}
