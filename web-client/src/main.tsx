import React from 'react'
import { createRoot } from 'react-dom/client'
import AppRoutes from './AppRoutes'
import './index.css'

createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <AppRoutes />
  </React.StrictMode>
)
