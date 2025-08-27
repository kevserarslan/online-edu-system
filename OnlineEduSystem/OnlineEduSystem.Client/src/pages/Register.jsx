import { useState } from "react";
import api from "../lib/axios";
import { useNavigate, Link } from "react-router-dom";

export default function Register(){
  const [fullName,setFullName]=useState("");
  const [userName,setUserName]=useState("");
  const [password,setPassword]=useState("");
  const [role,setRole]=useState("1"); // Instructor
  const [err,setErr]=useState("");
  const nav=useNavigate();

  const submit=async(e)=>{
    e.preventDefault(); setErr("");
    try{
      const dto={ fullName, userName, password, role:Number(role) };
      const res=await api.post("/api/auth/register",dto);
      if(res.data?.success) nav("/login");
      else setErr(res.data?.message || "Kayıt başarısız");
    }catch(ex){ setErr(ex.response?.data?.message || "Sunucu hatası"); }
  };

  return (
    <div className="center">
      <form onSubmit={submit} className="card auth-card">
        <div className="auth-title">Aramıza Katıl ✨</div>
        <div className="helper">Hesabını oluştur ve öğrenmeye başla.</div>

        <input className="input mt-2" placeholder="Ad Soyad" value={fullName} onChange={e=>setFullName(e.target.value)} />
        <div className="grid-2 mt-1">
          <input className="input" placeholder="Kullanıcı adı" value={userName} onChange={e=>setUserName(e.target.value)} />
          <input className="input" placeholder="Şifre" type="password" value={password} onChange={e=>setPassword(e.target.value)} />
        </div>

        <select className="input mt-1" value={role} onChange={e=>setRole(e.target.value)}>
          <option value="1">Instructor</option>
          <option value="2">Student</option>
          <option value="0">Admin</option>
        </select>

        <button className="btn btn-primary mt-2" type="submit">Kayıt Ol</button>
        <div className="helper mt-2">
          Zaten hesabın var mı? <Link to="/login">Giriş yap</Link>
        </div>
        {err && <div className="badge mt-2" style={{borderColor:"#7f1d1d", background:"#2a1416", color:"#fecaca"}}>{err}</div>}
      </form>
    </div>
  );
}
