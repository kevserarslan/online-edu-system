// src/pages/Courses.jsx
import { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import api from "../lib/axios";
import { getUser, isStudent, isInstructor, isAdmin } from "../lib/auth";

export default function Courses(){
  const me = getUser();

  const [list,setList] = useState([]);
  const [title,setTitle] = useState("");
  const [capacity,setCapacity] = useState(30);
  const [description,setDescription] = useState("");
  const [instructors,setInstructors] = useState([]);
  const [instructorId,setInstructorId] = useState("");
  const [enrollmentMap,setEnrollmentMap] = useState({});
  const [msg,setMsg] = useState("");

  // NEW: enrollments yükleniyor mu?
  const [enrollLoading, setEnrollLoading] = useState(true);

  const canCreateCourse = useMemo(()=>isInstructor(me) || isAdmin(me), [me]);
  const isStudentUser   = useMemo(()=>isStudent(me), [me]);

  const loadCourses = async () => {
    const r = await api.get("/api/courses");
    setList(r.data || []);
  };

  // Kayıtlarımı güvenli şekilde yükle (my endpoint + fallback)
  const loadMyEnroll = async () => {
    setEnrollLoading(true);
    if (!me) { setEnrollmentMap({}); setEnrollLoading(false); return; }

    // 1) Sadece kendi kayıtlarını dönen endpoint'i dene
    try {
      const r = await api.get("/api/enrollments/my");
      const mine = r.data || [];
      const map = {};
      for (const e of mine) map[e.courseId] = e.id ?? true;
      setEnrollmentMap(map);
    } catch {
      // 2) Fallback: Tüm kayıtları al, client-side filtrele
      try {
        const rAll = await api.get("/api/enrollments");
        const mine = (rAll.data || []).filter(e => e.userId === me.id);
        const map = {};
        for (const e of mine) map[e.courseId] = e.id ?? true;
        setEnrollmentMap(map);
      } catch {
        setEnrollmentMap({});
      }
    } finally {
      setEnrollLoading(false);
    }
  };

  const loadInstructors = async () => {
    const r = await api.get("/api/users/instructors");
    const arr = r.data || [];
    setInstructors(arr);
    if (isInstructor(me)) setInstructorId(me.id);
    else if (arr.length && !instructorId) setInstructorId(arr[0].id);
  };

  useEffect(() => {
    loadCourses();
    loadInstructors();
    loadMyEnroll();
    setMsg("");
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // me değiştiğinde (login/logout) kayıtları yeniden yükle
  useEffect(() => {
    loadMyEnroll();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [me?.id]);

  const create = async () => {
    setMsg("");
    if (!title.trim() || !instructorId) return;
    await api.post("/api/courses", {
      title,
      description,
      capacity: Number(capacity),
      instructorId
    });
    setTitle(""); setDescription(""); setCapacity(30);
    await loadCourses();
  };

  const removeCourse = async (id) => {
    setMsg("");
    await api.delete(`/api/courses/${id}`);
    setList(v => v.filter(x => x.id !== id));
  };

  const enroll = async (courseId) => {
    if (!me) { setMsg("Önce giriş yapın"); return; }
    try {
      const r = await api.post("/api/enrollments", { courseId, userId: me.id });
      setEnrollmentMap(m => ({ ...m, [courseId]: r.data?.id ?? true }));
      setMsg("Kursa katılım başarılı ✅");
      // İstersen otomatik materyallere yönlendir:
      // navigate(`/courses/${courseId}/materials`);
    } catch (e) {
      if (e.response?.status === 409) {
        setMsg(e.response?.data?.message || "Bu kursa zaten kayıtlısınız.");
        setEnrollmentMap(m => ({ ...m, [courseId]: true }));
      } else {
        setMsg(e.response?.data?.message || "Kayıt başarısız");
      }
    }
  };

  return (
    <div>
      <h2 style={{ margin: "0 0 14px" }}>Kurslar</h2>

      {canCreateCourse && (
        <div className="card">
          <div className="card-title">Kurs Oluştur</div>
          <div className="grid-2">
            <input className="input" placeholder="Başlık" value={title} onChange={e=>setTitle(e.target.value)} />
            <input className="input" placeholder="Kontenjan" type="number" value={capacity} onChange={e=>setCapacity(e.target.value)} />
          </div>
          <textarea className="input mt-1" placeholder="Açıklama" value={description} onChange={e=>setDescription(e.target.value)} />
          <div className="grid-2 mt-1">
            <select className="input" value={instructorId} onChange={e=>setInstructorId(e.target.value)} disabled={isInstructor(me)}>
              {isInstructor(me) && <option value={me?.id}>{me?.userName} (Ben)</option>}
              {!isInstructor(me) && instructors.map(i=>(
                <option key={i.id} value={i.id}>{i.fullName} ({i.userName})</option>
              ))}
            </select>
            <button className="btn btn-primary" onClick={create}>Kurs Oluştur</button>
          </div>
        </div>
      )}

      {msg && (
        <div
          className="badge mt-2"
          style={{ borderColor:"#14532d", background:"#0f2b1a", color:"#bbf7d0" }}
        >
          {msg}
        </div>
      )}

      <div className="mt-2" style={{ display:"grid", gap:12 }}>
        {list.map(c => {
          const already = !!enrollmentMap[c.id];
          const canViewMaterials = !isStudentUser || already;

          return (
            <div className="card course-card" key={c.id}>
              <div className="row" style={{ alignItems:"flex-start" }}>
                <div>
                  <div style={{ fontWeight:600, fontSize:17 }}>{c.title}</div>
                  <div className="meta">
                    Eğitmen: {c.instructorName || "—"} · Kayıt: {c.currentEnrollmentCount ?? "-"}
                  </div>
                  {c.description && (
                    <div className="mt-1" style={{ whiteSpace:"pre-wrap" }}>
                      {c.description}
                    </div>
                  )}
                </div>

                <div className="actions">
                  {enrollLoading ? (
                    <button className="btn btn-ghost" disabled>
                      Materyaller (kontrol ediliyor…)
                    </button>
                  ) : canViewMaterials ? (
                    <Link className="btn btn-ghost" to={`/courses/${c.id}/materials`}>Materyaller</Link>
                  ) : (
                    <button className="btn btn-ghost" disabled title="Önce kursa katıl">
                      🔒 Materyaller
                    </button>
                  )}

                  {isStudentUser && !already && (
                    <button className="btn btn-primary" onClick={()=>enroll(c.id)}>Kursa Katıl</button>
                  )}
                  {isStudentUser && already && (
                    <button className="btn btn-success" disabled>Kayıtlısın</button>
                  )}

                  {(isInstructor(me) || isAdmin(me)) && (
                    <button className="btn btn-danger" onClick={()=>removeCourse(c.id)}>Sil</button>
                  )}
                </div>
              </div>
            </div>
          );
        })}
      </div>

      {list.length === 0 && <div className="helper mt-2">Henüz kurs yok.</div>}
    </div>
  );
}
