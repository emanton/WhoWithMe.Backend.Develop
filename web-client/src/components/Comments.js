import React, { useEffect, useState, useContext } from 'react';
import { AuthContext } from '../AuthProvider';

export default function Comments({ meetingId }) {
  const { authFetch, token } = useContext(AuthContext);
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [text, setText] = useState('');

  useEffect(() => { load(); }, [meetingId]);

  const load = async () => {
    setLoading(true);
    try {
      const res = await authFetch('/Comment/GetMeetingComments', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ MeetingId: meetingId, Count: 50, Offset: 0 }) });
      const body = await res.json();
      setComments(body.data || []);
    } catch (e) { console.error(e); }
    finally { setLoading(false); }
  };

  const add = async () => {
    if (!text) return;
    try {
      const payload = { Estimation: 5, Creator: { Id: 0 }, Meeting: { Id: meetingId } };
      const res = await authFetch('/Comment/AddMeetingComment', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
      const body = await res.json();
      if (res.ok) { setText(''); load(); }
    } catch (e) { console.error(e); }
  };

  const remove = async (id) => {
    try {
      const payload = { Id: id };
      await authFetch('/Comment/DeleteMeetingComment', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
      load();
    } catch (e) { console.error(e); }
  };

  return (
    <div className="card" style={{ marginTop: 12 }}>
      <h3>Comments</h3>
      {loading ? <div>Loading comments...</div> : (
        <ul className="list">
          {comments.map(c => (
            <li key={c.id}>
              <div>Estimation: {c.estimation}</div>
              <div>By: {c.creator?.nickname ?? c.creator?.id}</div>
              <div><button onClick={()=>remove(c.id)} className="btn secondary">Delete</button></div>
            </li>
          ))}
        </ul>
      )}

      {token && (
        <div style={{ marginTop: 8 }}>
          <textarea className="textarea" value={text} onChange={e=>setText(e.target.value)} placeholder="Write a comment" />
          <div style={{ display:'flex', gap:8 }}>
            <button className="btn" onClick={add}>Add comment</button>
          </div>
        </div>
      )}
    </div>
  );
}
