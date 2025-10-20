import React, { createContext, useState, useEffect, ReactNode } from 'react'
import api, { setToken } from './api'

interface User {
  id?: number
  nickname?: string
}

interface AuthContextValue {
  token: string | null
  user: User | null
  login: (email: string, password: string) => Promise<any>
  register: (email: string, password: string) => Promise<any>
  logout: () => void
  authFetch: (input: RequestInfo, init?: RequestInit) => Promise<Response>
}

export const AuthContext = createContext<AuthContextValue>({
  token: null,
  user: null,
  login: async () => ({}),
  register: async () => ({}),
  logout: () => {},
  authFetch: (input: RequestInfo, init?: RequestInit) => fetch(input, init)
})

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setTokenState] = useState<string | null>(() => localStorage.getItem('token'))
  const [user, setUser] = useState<User | null>(() => {
    const raw = localStorage.getItem('user')
    return raw ? JSON.parse(raw) : null
  })

  useEffect(() => {
    if (token) {
      setToken(token)
    }
  }, [token])

  useEffect(() => {
    if (token) {
      localStorage.setItem('token', token)
    } else {
      localStorage.removeItem('token')
    }
  }, [token])

  useEffect(() => {
    if (user) localStorage.setItem('user', JSON.stringify(user))
    else localStorage.removeItem('user')
  }, [user])

  const login = async (email: string, password: string) => {
    const res = await api.post('/Authorization/EmailLogin', { Email: email, Password: password })
    const body = res.data?.data ?? res.data
    if (body?.token) {
      setTokenState(body.token)
      setUser({ id: body.id, nickname: body.nickname })
      setToken(body.token)
    }
    return body
  }

  const register = async (email: string, password: string) => {
    const res = await api.post('/Authorization/EmailRegister', { Email: email, Password: password })
    const body = res.data?.data ?? res.data
    if (body?.token) {
      setTokenState(body.token)
      setUser({ id: body.id, nickname: body.nickname })
      setToken(body.token)
    }
    return body
  }

  const logout = () => {
    setTokenState(null)
    setUser(null)
    // clear axios header
    setToken('')
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  const authFetch = (input: RequestInfo, init: RequestInit = {}) => {
    const headers = init.headers ? { ...(init.headers as any) } : {}
    if (token) headers['Authorization'] = `Bearer ${token}`
    if (!headers['Content-Type']) headers['Content-Type'] = 'application/json'
    return fetch(input, { ...init, headers })
  }

  return (
    <AuthContext.Provider value={{ token, user, login, register, logout, authFetch }}>
      {children}
    </AuthContext.Provider>
  )
}
