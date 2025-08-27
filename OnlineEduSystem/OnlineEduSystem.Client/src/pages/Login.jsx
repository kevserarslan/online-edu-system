import { useState } from "react";
import api from "../lib/axios";
import { useNavigate, Link } from "react-router-dom";
import { setToken } from "../lib/auth";

export default function Login(){
  const [userName,setUserName]=useState("");
  const [password,setPassword]=useState("");
  const [err,setErr]=useState("");
  const nav = useNavigate();

  const handleLogin=async(e)=>{
    e.preventDefault(); setErr("");
    try{
      const res = await api.post("/api/auth/login",{ userName, password });
      if(res.data?.success){
        setToken(res.data.token);
        nav("/courses",{ replace:true });
      }else setErr(res.data?.message || "Giriş başarısız");
    }catch(ex){ setErr(ex.response?.data?.message || "Sunucu hatası"); }
  };

  return (
    <div className="center">
      <form onSubmit={handleLogin} className="card auth-card">
        <div className="auth-title">Tekrar Hoş Geldin 👋</div>
        <div className="helper">Hesabına giriş yap ve derslerine devam et.</div>

        <div className="mt-2 grid-2">
          <input className="input" placeholder="Kullanıcı adı" value={userName} onChange={e=>setUserName(e.target.value)} />
          <input className="input" placeholder="Şifre" type="password" value={password} onChange={e=>setPassword(e.target.value)} />
        </div>

        <button type="submit" className="btn btn-primary mt-2">Giriş Yap</button>
        <div className="helper mt-2">
          Hesabın yok mu? <Link to="/register">Kayıt ol</Link>
        </div>
        {err && <div className="badge mt-2" style={{borderColor:"#7f1d1d", background:"#2a1416", color:"#fecaca"}}>{err}</div>}
      </form>
    </div>
  );
}
