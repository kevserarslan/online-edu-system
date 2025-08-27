// src/pages/MyCourses.jsx
import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../lib/axios";
import { getUser } from "../lib/auth";

export default function MyCourses() {
  const me = getUser();
  const nav = useNavigate();

  const [items, setItems]   = useState([]);   // { id, courseId, courseTitle, ... }
  const [loading, setLoad]  = useState(true);
  const [err, setErr]       = useState("");

  useEffect(() => {
    const load = async () => {
      setErr(""); setLoad(true);
      try {
        // Tercih edilen: sadece kendi kayıtlarını dönen endpoint
        const r = await api.get("/api/enrollments/my");
        setItems(r.data || []);
      } catch (e1) {
        // Fallback: tüm kayıtları al, front-end’de filtrele
        try {
          const rAll = await api.get("/api/enrollments");
          const all = rAll.data || [];
          const mine = all.filter(e =>
            // bazı API'lar userId döndürür, bazıları userName
            (e.userId ? e.userId === me?.id : e.userName === me?.userName)
          );
          setItems(mine);
        } catch (e2) {
          setErr(e2.response?.data?.message || "Kayıtlar yüklenemedi");
        }
      } finally {
        setLoad(false);
      }
    };
    load();
  }, [me?.id, me?.userName]);

  return (
    <div>
      <div className="row" style={{ marginBottom: 12 }}>
        <h2 style={{ margin: 0 }}>Kayıtlı Kurslarım</h2>
        <div className="nav-spacer" />
        <button className="btn btn-ghost" onClick={() => nav("/courses")}>← Tüm Kurslar</button>
      </div>

      {err && (
        <div className="badge mt-1" style={{ borderColor:"#7f1d1d", background:"#2a1416", color:"#fecaca" }}>
          {err}
        </div>
      )}

      {loading ? (
        <div className="helper">Yükleniyor…</div>
      ) : items.length === 0 ? (
        <div className="helper">Kayıtlı kursun yok.</div>
      ) : (
        <div className="mt-2" style={{ display:"grid", gap:12 }}>
          {items.map(enr => (
            <div key={enr.id} className="card course-card">
              <div className="row" style={{ alignItems: "center" }}>
                <div>
                  <div style={{ fontWeight: 600, fontSize: 17 }}>
                    {enr.courseTitle || `Kurs #${enr.courseId}`}
                  </div>
                  <div className="meta">
                    Kayıt tarihi: {enr.enrolledAt ? new Date(enr.enrolledAt).toLocaleString() : "—"}
                  </div>
                </div>
                <div className="actions">
                  <Link className="btn btn-ghost" to={`/courses/${enr.courseId}/materials`}>
                    Materyaller
                  </Link>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
