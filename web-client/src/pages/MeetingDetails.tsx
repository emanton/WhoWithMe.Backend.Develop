import React, { useEffect, useState } from 'react'
import { useParams, Link } from 'react-router-dom'
import api from '../api'
import Comments from '../components/Comments'

export default function MeetingDetails(){
  const { id } = useParams()
  const [meeting, setMeeting] = useState<any>(null)

  useEffect(() => { if (!id) return; load() }, [id])

  async function load(){
    try {
      const res = await api.post('/Meeting/GetMeeting', { MeetingId: Number(id) })
      const data = res.data?.data ?? res.data?.Data
      setMeeting(data)
    } catch (err) { console.error(err) }
  }

  if (!meeting) return <div className="card">Loading...</div>

  return (
    <div className="card">
      <h2>{meeting.title}</h2>
      <p>{meeting.description}</p>
      <p><strong>Start:</strong> {meeting.startDate ? new Date(meeting.startDate).toLocaleString() : '—'}</p>
      <p><strong>Address:</strong> {meeting.address}</p>
      <p><strong>Participants:</strong> {meeting.participantsCount}</p>
      <p><strong>Subscribers:</strong> {meeting.subscribersCount}</p>

      <Link to="/">Back</Link>

      <Comments meetingId={Number(id)} />
    </div>
  )
}
