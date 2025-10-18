import React, { useState } from 'react'
import api from '../api'
import { useNavigate } from 'react-router-dom'

export default function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [message, setMessage] = useState('')
  const navigate = useNavigate()

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    try {
      const res = await api.post('/Authorization/EmailLogin', { email, password })
      const token = res.data?.data?.token
      if (token) {
        localStorage.setItem('token', token)
        api.defaults.headers.common['Authorization'] = `Bearer ${token}`
        navigate('/')
      } else {
        setMessage('Login failed')
      }
    } catch (err: any) {
      setMessage(err?.response?.data?.error ?? 'Error')
    }
  }

  return (
    <div className="container">
      <div className="card">
        <form onSubmit={submit}>
          <h2>Login</h2>
          <div className="form-row">
            <label>Email</label>
            <input value={email} onChange={e => setEmail(e.target.value)} />
          </div>
          <div className="form-row">
            <label>Password</label>
            <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
          </div>
          <button className="button" type="submit">Login</button>
          <div className="message">{message}</div>
        </form>
      </div>
    </div>
  )
}
