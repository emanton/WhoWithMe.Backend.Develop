import React, { useEffect, useState } from 'react'
import api from '../api'

export default function Admin() {
  const [cities, setCities] = useState<string[]>([])
  const [name, setName] = useState('')
  const [types, setTypes] = useState<string[]>([])
  const [typeName, setTypeName] = useState('')

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

  async function addCity(e: React.FormEvent) {
    e.preventDefault()
    try {
      await api.post('/Dictionaries/AddCity', { name })
      setName('')
      fetchLists()
    } catch (err) { console.error(err) }
  }

  async function addType(e: React.FormEvent) {
    e.preventDefault()
    try {
      await api.post('/Dictionaries/AddMeetingType', { name: typeName })
      setTypeName('')
      fetchLists()
    } catch (err) { console.error(err) }
  }

  return (
    <div className="container">
      <div className="card">
        <h2>Admin</h2>
        <div className="two-column">
          <div className="col">
            <h3>Cities</h3>
            <ul className="list">{cities.map((c:any) => <li key={c.id}>{c.name}</li>)}</ul>
            <form onSubmit={addCity} className="form-row">
              <input value={name} onChange={e => setName(e.target.value)} placeholder="City name" />
              <button className="button" type="submit">Add City</button>
            </form>
          </div>

          <div className="col">
            <h3>Meeting Types</h3>
            <ul className="list">{types.map((t:any) => <li key={t.id}>{t.name}</li>)}</ul>
            <form onSubmit={addType} className="form-row">
              <input value={typeName} onChange={e => setTypeName(e.target.value)} placeholder="Type name" />
              <button className="button" type="submit">Add Type</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  )
}
