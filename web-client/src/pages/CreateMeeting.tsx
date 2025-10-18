import React, { useEffect, useState } from 'react'
import api from '../api'

export default function CreateMeeting() {
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [cityId, setCityId] = useState<number | undefined>()
  const [typeId, setTypeId] = useState<number | undefined>()
  const [cities, setCities] = useState<any[]>([])
  const [types, setTypes] = useState<any[]>([])

  useEffect(() => { fetchLists() }, [])

  async function fetchLists() {
    try {
      const c = await api.post('/Dictionaries/GetCities')
      setCities(c.data?.data ?? [])
      const t = await api.post('/Dictionaries/GetMeetingTypes')
      setTypes(t.data?.data ?? [])
    } catch (err) {
      console.error(err)
    }
  }

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    try {
      const payload = {
        title,
        description,
        creatorId: 1, // demo: replace with current user id
        cityId: cityId,
        meetingTypeId: typeId,
        createdDate: new Date(),
        startDate: new Date(Date.now() + 1000 * 60 * 60 * 24)
      }
      await api.post('/Meeting/AddMeeting', payload)
      setTitle('')
      setDescription('')
    } catch (err) { console.error(err) }
  }

  return (
    <div className="container">
      <div className="card">
        <form onSubmit={submit}>
          <h2>Create Meeting</h2>

          <div className="form-row">
            <label>Title</label>
            <input value={title} onChange={e => setTitle(e.target.value)} />
          </div>

          <div className="form-row">
            <label>Description</label>
            <textarea value={description} onChange={e => setDescription(e.target.value)} />
          </div>

          <div className="form-row row">
            <div className="col">
              <label>City</label>
              <select onChange={e => setCityId(Number(e.target.value))}>
                <option value="">Select</option>
                {cities.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
              </select>
            </div>
            <div className="col">
              <label>Type</label>
              <select onChange={e => setTypeId(Number(e.target.value))}>
                <option value="">Select</option>
                {types.map(t => <option key={t.id} value={t.id}>{t.name}</option>)}
              </select>
            </div>
          </div>

          <button className="button" type="submit">Create</button>
        </form>
      </div>
    </div>
  )
}
