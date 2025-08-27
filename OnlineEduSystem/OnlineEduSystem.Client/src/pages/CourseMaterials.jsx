import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import api from "../lib/axios";
import { getUser, isInstructor, isAdmin } from "../lib/auth";

export default function CourseMaterials(){
  const { id:courseId } = useParams();
  const [materials,setMaterials]=useState([]);
  const [loading,setLoading]=useState(true);
  const [err,setErr]=useState("");

  const [title,setTitle]=useState("");
  const [type,setType]=useState("Pdf");
  const [file,setFile]=useState(null);
  const [uploadMsg,setUploadMsg]=useState("");

  const user = getUser();
  const canUpload = isInstructor(user) || isAdmin(user);

  const apiBase=(import.meta.env.VITE_API_BASE_URL||"").replace(/\/$/,"");
  const fileUrl=(p)=>(p?.startsWith("http")?p:`${apiBase}${p}`);

  const load=async()=>{
    setErr(""); setLoading(true);
    try{
      const r=await api.get(`/api/courses/${courseId}/materials`);
      setMaterials(r.data||[]);
    }catch(e){
      const status = e.response?.status;
      if(status===401) setErr("Giriş gerekli. Lütfen giriş yapın.");
      else if(status===403) setErr("🔒 Bu kursun materyallerini görmek için önce kursa katılın.");
      else setErr(e.response?.data?.message || "Liste alınamadı");
    }
    finally{ setLoading(false); }
  };
  useEffect(()=>{ load(); },[courseId]);

  const upload=async()=>{
    if(!canUpload) return;
    setUploadMsg(""); setErr("");
    if(!title.trim()||!file){ setErr("Başlık ve dosya zorunlu"); return; }
    const fd=new FormData(); fd.append("title",title); fd.append("type",type); fd.append("file",file);
    try{
      await api.post(`/api/courses/${courseId}/materials/upload`, fd);
      setTitle(""); setType("Pdf"); setFile(null);
      setUploadMsg("Yüklendi ✅"); await load();
    }catch(e){
      const status = e.response?.status;
      if(status===401) setErr("Giriş gerekli. Lütfen giriş yapın.");
      else if(status===403) setErr("Bu işlem için yetkiniz yok.");
      else setErr(e.response?.data?.message || "Yükleme başarısız");
    }
  };

  const removeItem=async(mid)=>{
    if(!canUpload) return;
    if(!confirm("Silinsin mi?")) return;
    try{
      await api.delete(`/api/courses/${courseId}/materials/${mid}`);
      setMaterials(m=>m.filter(x=>x.id!==mid));
    }catch(e){ alert(e.response?.data?.message || "Silme hatası"); }
  };

  return (
    <div>
      <div className="row" style={{marginBottom:12}}>
        <h2 style={{margin:0}}>Kurs Materyalleri</h2>
        <div className="nav-spacer" />
        <Link className="btn btn-ghost" to="/courses">← Kurslara dön</Link>
      </div>

      {/* Genel hata/uyarı mesajı (herkes görsün) */}
      {err && (
        <div className="badge mt-1" style={{borderColor:"#7f1d1d", background:"#2a1416", color:"#fecaca"}}>
          {err}
        </div>
      )}

      {canUpload && (
        <div className="card">
          <div className="card-title">Dosya Yükle (PDF / Video / Document)</div>
          <div className="grid-2">
            <input className="input" placeholder="Başlık" value={title} onChange={e=>setTitle(e.target.value)} />
            <select className="input" value={type} onChange={e=>setType(e.target.value)}>
              <option value="Pdf">Pdf</option>
              <option value="Video">Video</option>
              <option value="Document">Document</option>
            </select>
          </div>
          <input className="input mt-1" type="file"
                 accept=".pdf,.doc,.docx,.ppt,.pptx,.mp4,.mov,.avi,.mkv"
                 onChange={e=>setFile(e.target.files?.[0]??null)} />
          <div className="row mt-1">
            <button className="btn btn-primary" onClick={upload}>Yükle</button>
            {uploadMsg && <span className="badge">{uploadMsg}</span>}
            {/* Upload formundaki hatayı da burada gösterelim */}
            {!uploadMsg && err && <span className="badge" style={{borderColor:"#7f1d1d", background:"#2a1416", color:"#fecaca"}}>{err}</span>}
          </div>
        </div>
      )}

      <div className="mt-2" style={{display:"grid", gap:12}}>
        {loading ? (
          <div className="helper">Yükleniyor…</div>
        ) : err ? (
          // Liste hatasında da görünür olsun
          <div className="helper">{err}</div>
        ) : (
          materials.length === 0 ? <div className="helper">Henüz materyal yok.</div> :
          materials.map(m=>{
            const typeText = typeof m.type==="string" ? m.type : ["Pdf","Video","Document"][m.type];
            return (
              <div key={m.id} className="card row" style={{justifyContent:"space-between"}}>
                <div className="row" style={{gap:10}}>
                  <span className="badge">{typeText}</span>
                  <div>
                    <div style={{fontWeight:600}}>{m.title}</div>
                    <div className="meta">{new Date(m.uploadedAt).toLocaleString()}</div>
                  </div>
                </div>
                <div className="row">
                  <a className="btn btn-ghost" href={fileUrl(m.filePath)} target="_blank" rel="noreferrer">Aç</a>
                  {canUpload && <button className="btn btn-danger" onClick={()=>removeItem(m.id)}>Sil</button>}
                </div>
              </div>
            );
          })
        )}
      </div>
    </div>
  );
}
