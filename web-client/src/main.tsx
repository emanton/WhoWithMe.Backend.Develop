import React from 'react'
import { createRoot } from 'react-dom/client'
import AppRoutes from './AppRoutes'
import './index.css'
import { AuthProvider } from './AuthProvider'

createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <AuthProvider>
      <AppRoutes />
    </AuthProvider>
  </React.StrictMode>
)
