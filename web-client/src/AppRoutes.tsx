import React, { useContext } from 'react'
import { BrowserRouter, Routes, Route, Link, useNavigate } from 'react-router-dom'
import App from './App'
import Register from './pages/Register'
import Login from './pages/Login'
import Admin from './pages/Admin'
import CreateMeeting from './pages/CreateMeeting'
import MeetingDetails from './pages/MeetingDetails'
import { AuthContext } from './AuthProvider'

function NavLinks() {
  const { token, user, logout } = useContext<any>(AuthContext)
  const navigate = useNavigate()

  const doLogout = () => {
    logout()
    navigate('/')
  }

  return (
    <div className="nav-inner">
      <div className="nav-logo">WhoWithMe</div>
      <div className="nav-links">
        <Link to="/">Home</Link>
        <Link to="/create">Create</Link>
        <Link to="/admin">Admin</Link>
        {!token && (
          <>
            <Link to="/register">Register</Link>
            <Link to="/login">Login</Link>
          </>
        )}
        {token && (
          <>
            <span style={{ marginLeft: 12, marginRight: 8 }}>Hi, {user?.nickname ?? user?.id ?? 'you'}</span>
            <button className="button" onClick={doLogout}>Logout</button>
          </>
        )}
      </div>
    </div>
  )
}

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <div className="container">
        <nav className="card">
          <NavLinks />
        </nav>

        <Routes>
          <Route path="/" element={<App />} />
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />
          <Route path="/admin" element={<Admin />} />
          <Route path="/create" element={<CreateMeeting />} />
          <Route path="/meeting/:id" element={<MeetingDetails />} />
        </Routes>

        <div className="footer">Demo project</div>
      </div>
    </BrowserRouter>
  )
}
