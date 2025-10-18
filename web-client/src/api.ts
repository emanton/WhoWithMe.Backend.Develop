import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api'
})

const token = localStorage.getItem('token')
if (token) api.defaults.headers.common['Authorization'] = `Bearer ${token}`

export function setToken(t: string) {
  localStorage.setItem('token', t)
  api.defaults.headers.common['Authorization'] = `Bearer ${t}`
}

export default api
