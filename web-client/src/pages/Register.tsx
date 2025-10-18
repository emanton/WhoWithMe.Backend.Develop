import React, { useState } from 'react'
import api from '../api'
import '../styles/register.css'

export default function Register() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [message, setMessage] = useState('')

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    try {
      const res = await api.post('/Authorization/EmailRegister', { email, password })
      setMessage(res.data?.data?.token ? 'Registered and logged in' : 'Registered')
    } catch (err: any) {
      setMessage(err?.response?.data?.error ?? 'Error')
    }
  }

  return (
    <div className="container">
      <div className="card">
        <form onSubmit={submit}>
          <h2>Register</h2>
          <div className="form-row">
            <label>Email</label>
            <input value={email} onChange={e => setEmail(e.target.value)} />
          </div>
          <div className="form-row">
            <label>Password</label>
            <input type="password" value={password} onChange={e => setPassword(e.target.value)} />
          </div>
          <button className="button" type="submit">Register</button>
          <div className="message">{message}</div>
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
