import React, { useState, useContext } from 'react'
import { useNavigate } from 'react-router-dom'
import { AuthContext } from '../AuthProvider'

export default function Login() {
  const { login } = useContext<any>(AuthContext)
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  const doLogin = async () => {
    setError(null)
    try {
      await login(email, password)
      navigate('/')
    } catch (e: any) {
      setError(e?.message ?? 'Login failed')
    }
  }

  return (
    <div className="card auth-form">
      <h2>Login</h2>
      {error && <div style={{ color: 'red' }}>{error}</div>}
      <input className="input" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
      <input className="input" placeholder="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
      <div style={{ display: 'flex', gap: 8 }}>
        <button className="btn" onClick={doLogin}>Login</button>
      </div>
    </div>
  )
}
