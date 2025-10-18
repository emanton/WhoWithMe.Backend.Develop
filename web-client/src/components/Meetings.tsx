import React, { useEffect, useState } from 'react'
import api from '../api'

interface Meeting {
  id: number
  title: string
  description?: string
}

export default function Meetings() {
  const [meetings, setMeetings] = useState<Meeting[]>([])

  useEffect(() => {
    ;(async () => {
      try {
        const res = await api.post('/Meeting/GetMeetingsByOwner', { userId: 1, page: 0, pageSize: 10 })
        // backend returns a wrapper: { statusCode, errorMessage, data }
        console.debug('GetMeetingsByOwner response', res.data)
        const payload = res.data?.data ?? res.data?.Data ?? []
        setMeetings(Array.isArray(payload) ? payload : [])
      } catch (err) {
        console.error(err)
      }
    })()
  }, [])

  return (
    <div className="container">
      <div className="card">
        <h2>Meetings</h2>
        {meetings.length === 0 && <div>No meetings</div>}
        <ul className="list">
          {meetings.map(m => (
            <li key={m.id}>
              <div className="meeting-title">{m.title}</div>
              <div className="meeting-desc">{m.description}</div>
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
