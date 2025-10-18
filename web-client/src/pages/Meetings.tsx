import React, { useEffect, useState } from 'react'
import api from '../api'

interface Meeting {
  id: number
  title: string
  description?: string
}

interface MeetingType {
  id: number
  name: string
}

export default function Meetings() {
  const [meetings, setMeetings] = useState<Meeting[]>([])
  const [types, setTypes] = useState<MeetingType[]>([])
  const [selectedTypeIds, setSelectedTypeIds] = useState<number[]>([])
  const [title, setTitle] = useState('')
  const [loading, setLoading] = useState(false)

  useEffect(() => {
    fetchTypes()
    // initial search
    search()
  }, [])

  async function fetchTypes() {
    try {
      const res = await api.post('/Dictionaries/GetMeetingTypes')
      const payload = res.data?.data ?? res.data?.Data ?? []
      setTypes(Array.isArray(payload) ? payload : [])
    } catch (err) {
      console.error('Failed to load meeting types', err)
    }
  }

  async function search(offset = 0) {
    setLoading(true)
    try {
      const payload: any = {
        meetingTypeIds: selectedTypeIds,
        count: 20,
        offset: offset
      }
      // include title if provided (backend may ignore unknown fields if DTO doesn't contain title)
      if (title && title.trim().length > 0) payload.title = title.trim()

      const res = await api.post('/Meeting/GetMeetingsByTitleAndType', payload)
      console.debug('GetMeetingsByTitleAndType response', res.data)
      const data = res.data?.data ?? res.data?.Data ?? []
      setMeetings(Array.isArray(data) ? data : [])
    } catch (err) {
      console.error(err)
      setMeetings([])
    } finally {
      setLoading(false)
    }
  }

  function toggleType(id: number) {
    setSelectedTypeIds(prev => prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id])
  }

  return (
    <div className="container">
      <div className="card">
        <h2>Search Meetings</h2>

        <div className="form-row">
          <label>Title</label>
          <input value={title} onChange={e => setTitle(e.target.value)} placeholder="Search by title" />
        </div>

        <div className="form-row">
          <label>Types</label>
          <div style={{ display: 'flex', gap: 8, flexWrap: 'wrap' }}>
            {types.map(t => (
              <label key={t.id} style={{ display: 'flex', alignItems: 'center', gap: 6 }}>
                <input type="checkbox" checked={selectedTypeIds.includes(t.id)} onChange={() => toggleType(t.id)} />
                <span>{t.name}</span>
              </label>
            ))}
          </div>
        </div>

        <div style={{ display: 'flex', gap: 8 }}>
          <button className="button" onClick={() => search()}>Search</button>
          <button className="button secondary" onClick={() => { setTitle(''); setSelectedTypeIds([]); search(); }}>Reset</button>
        </div>

      </div>

      <div className="card">
        <h2>Results {loading && '(loading...)'}</h2>
        {meetings.length === 0 && !loading && <div>No meetings</div>}
        <ul className="list">
          {meetings.map((m: any) => (
            <li key={m.id ?? m.Id}>
              <div className="meeting-title">{m.title ?? m.Title}</div>
              <div className="meeting-desc">{m.description ?? m.Description}</div>
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
