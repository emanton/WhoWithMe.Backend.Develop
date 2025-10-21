import React, { useState, useContext } from 'react'
import { useNavigate } from 'react-router-dom'
import { AuthContext } from '../AuthProvider'
import '../styles/register.css'

export default function Register() {
  const { register } = useContext<any>(AuthContext)
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  const doRegister = async () => {
    setError(null)
    try {
      await register(email, password)
      navigate('/')
    } catch (e: any) {
      setError(e?.message ?? 'Registration failed')
    }
  }

  return (
    <div className="container">
      <div className="card auth-form">
        <form onSubmit={e => { e.preventDefault(); doRegister() }}>
          <h2>Register</h2>
          {error && <div style={{ color: 'red' }}>{error}</div>}
          <div className="form-row">
            <label>Email</label>
            <input className="input" value={email} onChange={e => setEmail(e.target.value)} />
          </div>
          <div className="form-row">
            <label>Password</label>
            <input className="input" type="password" value={password} onChange={e => setPassword(e.target.value)} />
          </div>
          <button className="button" type="submit">Register</button>
          <div className="register-note">By registering you accept terms.</div>
          <div className="register-helpers">
            <a href="/login">Already have account?</a>
            <a href="/">Privacy policy</a>
          </div>
        </form>
      </div>
    </div>
  )
}
