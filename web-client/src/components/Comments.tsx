import React, { useEffect, useState, useContext } from 'react'
import { AuthContext } from '../AuthProvider'
import api from '../api'

interface Creator {
  id?: number
  nickname?: string
}

interface Comment {
  id: number
  estimation?: number
  text?: string
  creator?: Creator
}

interface Props {
  meetingId: number
}

export default function Comments({ meetingId }: Props) {
  const { token, user } = useContext<any>(AuthContext)
  const [comments, setComments] = useState<Comment[]>([])
  const [loading, setLoading] = useState(false)
  const [text, setText] = useState('')

  useEffect(() => {
    load()
  }, [meetingId])

  const load = async () => {
    setLoading(true)
    try {
      const res = await api.post('/Comment/GetMeetingComments', { MeetingId: meetingId, Count: 50, Offset: 0 })
      const body = res.data?.data ?? res.data?.Data ?? []
      setComments(Array.isArray(body) ? body : [])
    } catch (e) {
      console.error(e)
    } finally {
      setLoading(false)
    }
  }

  const add = async () => {
    const trimmed = (text || '').trim()
    if (!trimmed) return
    try {
      const creatorId = user?.id || (localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user') || '{}').id : 0)
      const payload = { Estimation: 5, Text: trimmed, Creator: { Id: creatorId || 0 }, Meeting: { Id: meetingId } }
      const res = await authFetch('/Comment/AddMeetingComment', payload)
      const ok = res.status >= 200 && res.status < 300
      if (ok) {
        setText('')
        await load()
      } else {
        console.error('Failed to add comment', res.data)
      }
    } catch (e) {
      console.error(e)
    }
  }

  const remove = async (id: number) => {
    try {
      const payload = { Id: id }
      const res = await api.post('/Comment/DeleteMeetingComment', payload)
      const ok = res.status >= 200 && res.status < 300
      if (ok) await load()
      else console.error('Failed to delete comment', res.data)
    } catch (e) {
      console.error(e)
    }
  }

  const isAuthenticated = Boolean(token || localStorage.getItem('token'))

  return (
    <div className="card" style={{ marginTop: 12 }}>
      <h3>Comments</h3>
      {loading ? (
        <div>Loading comments...</div>
      ) : (
        <ul className="list">
          {comments.map((c) => (
            <li key={c.id}>
              <div>{c.text}</div>
              <div>Estimation: {c.estimation ?? '-'}</div>
              <div>By: {c.creator?.nickname ?? c.creator?.id ?? 'unknown'}</div>
              <div>
                <button onClick={() => remove(c.id)} className="btn secondary">
                  Delete
                </button>
              </div>
            </li>
          ))}
        </ul>
      )}

      {isAuthenticated && (
        <div style={{ marginTop: 8 }}>
          <textarea className="textarea" value={text} onChange={(e) => setText(e.target.value)} placeholder="Write a comment" />
          <div style={{ display: 'flex', gap: 8 }}>
            <button className="btn" onClick={add}>
              Add comment
            </button>
          </div>
        </div>
      )}

      {!isAuthenticated && <div style={{ marginTop: 8 }}>Please log in to add comments.</div>}
    </div>
  )
}
