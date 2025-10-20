import React from 'react'
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import App from './App'
import Register from './pages/Register'
import Login from './pages/Login'
import Admin from './pages/Admin'
import CreateMeeting from './pages/CreateMeeting'
import MeetingDetails from './pages/MeetingDetails'

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <div className="container">
        <nav className="card">
          <div className="nav-inner">
            <div className="nav-logo">WhoWithMe</div>
            <div className="nav-links">
              <Link to="/">Home</Link>
              <Link to="/register">Register</Link>
              <Link to="/login">Login</Link>
              <Link to="/admin">Admin</Link>
              <Link to="/create">Create</Link>
            </div>
          </div>
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
