import React, { useEffect, useState, useContext } from 'react'
import { AuthContext } from '../AuthProvider'

interface Creator {
  id?: number
  nickname?: string
}

interface Comment {
  id: number
  estimation?: number
  creator?: Creator
}

interface Props {
  meetingId: number
}

export default function Comments({ meetingId }: Props) {
  const { authFetch, token } = useContext<any>(AuthContext)
  const [comments, setComments] = useState<Comment[]>([])
  const [loading, setLoading] = useState(false)
  const [text, setText] = useState('')

  useEffect(() => {
    load()
  }, [meetingId])

  const load = async () => {
    setLoading(true)
    try {
      const res = await authFetch('/Comment/GetMeetingComments', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ MeetingId: meetingId, Count: 50, Offset: 0 })
      })
      const body = await res.json()
      const payload = body?.data ?? body?.Data ?? []
      setComments(Array.isArray(payload) ? payload : [])
    } catch (e) {
      console.error(e)
    } finally {
      setLoading(false)
    }
  }

  const add = async () => {
    if (!text) return
    try {
      // server CommentMeeting has no text field in schema; if you added one, send it here
      const payload = { Estimation: 5, /*Text: text,*/ Creator: { Id: 0 }, Meeting: { Id: meetingId } }
      const res = await authFetch('/Comment/AddMeetingComment', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      })
      if (res.ok) {
        setText('')
        await load()
      } else {
        console.error('Failed to add comment', await res.text())
      }
    } catch (e) {
      console.error(e)
    }
  }

  const remove = async (id: number) => {
    try {
      const payload = { Id: id }
      const res = await authFetch('/Comment/DeleteMeetingComment', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      })
      if (res.ok) await load()
      else console.error('Failed to delete comment', await res.text())
    } catch (e) {
      console.error(e)
    }
  }

  return (
    <div className="card" style={{ marginTop: 12 }}>
      <h3>Comments</h3>
      {loading ? (
        <div>Loading comments...</div>
      ) : (
        <ul className="list">
          {comments.map((c) => (
            <li key={c.id}>
              <div>Estimation: {c.estimation ?? c.estimation}</div>
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

      {token && (
        <div style={{ marginTop: 8 }}>
          <textarea className="textarea" value={text} onChange={(e) => setText(e.target.value)} placeholder="Write a comment" />
          <div style={{ display: 'flex', gap: 8 }}>
            <button className="btn" onClick={add}>
              Add comment
            </button>
          </div>
        </div>
      )}
    </div>
  )
}
