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
  authFetch: (input: RequestInfo, init?: RequestInit) => Promise<any>
}

export const AuthContext = createContext<AuthContextValue>({
  token: null,
  user: null,
  login: async () => ({}),
  register: async () => ({}),
  logout: () => {},
  authFetch: async () => ({})
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

  // authFetch implemented on top of axios api to keep a single HTTP client
  const authFetch = async (input: RequestInfo, init: RequestInit = {}) => {
    const url = typeof input === 'string' ? input : String(input)
    const method = (init.method || 'GET').toString().toLowerCase()

    // prepare data from fetch-style body
    let data: any = undefined
    if (init.body) {
      try {
        data = typeof init.body === 'string' ? JSON.parse(init.body as string) : init.body
      } catch {
        data = init.body
      }
    }

    const headers: any = init.headers ? { ...(init.headers as any) } : {}
    // attach token if present
    const currentToken = token ?? localStorage.getItem('token')
    if (currentToken) headers['Authorization'] = `Bearer ${currentToken}`
    if (!headers['Content-Type'] && !headers['content-type']) headers['Content-Type'] = 'application/json'

    try {
      const response = await api.request({ url, method, data, headers })
      // return a fetch-like response for compatibility with existing code
      return {
        ok: response.status >= 200 && response.status < 300,
        status: response.status,
        json: async () => response.data,
        text: async () => (typeof response.data === 'string' ? response.data : JSON.stringify(response.data)),
        data: response.data
      }
    } catch (err: any) {
      const resp = err?.response
      return {
        ok: false,
        status: resp?.status ?? 0,
        json: async () => resp?.data ?? null,
        text: async () => (resp ? JSON.stringify(resp.data) : String(err.message))
      }
    }
  }

  return (
    <AuthContext.Provider value={{ token, user, login, register, logout, authFetch }}>
      {children}
    </AuthContext.Provider>
  )
}
