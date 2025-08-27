// src/App.jsx
import { BrowserRouter, Routes, Route, Navigate, Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

import Login from "./pages/Login.jsx";
import Register from "./pages/Register.jsx";
import Courses from "./pages/Courses.jsx";
import CourseMaterials from "./pages/CourseMaterials.jsx";
import MyCourses from "./pages/MyCourses.jsx";            // ⬅️ EKLENDİ

import { isLoggedIn, getUserFromToken, logout, onAuthChange } from "./lib/auth";
import "./index.css";

function Navbar(){
  const nav = useNavigate();
  const [me, setMe] = useState(getUserFromToken());

  useEffect(() => {
    // token değişince kullanıcıyı yeniden oku
    const off = onAuthChange(() => setMe(getUserFromToken()));
    return off;
  }, []);

  const handleLogout = () => {
    logout();
    nav("/login", { replace:true });
  };

  return (
    <div className="navbar">
      <div className="container row">
        <Link className="brand" to="/courses">OnlineEdu</Link>
        <div className="nav-spacer" />
        <Link to="/courses">Kurslar</Link>

        {/* ⬇️ Oturum açıkken Kayıtlı Kurslarım linki */}
        {me && <Link to="/my-courses" style={{ marginLeft:12 }}>Kayıtlı Kurslarım</Link>}

        {!me ? (
          <div className="row" style={{ marginLeft:12 }}>
            <Link to="/login">Giriş</Link>
            <span className="tag">·</span>
            <Link to="/register">Kayıt</Link>
          </div>
        ) : (
          <div className="row" style={{ marginLeft:12 }}>
            <span className="user-pill">
              {me.userName} <span className="tag">({me.role})</span>
            </span>
            <button className="btn btn-ghost" onClick={handleLogout}>Çıkış</button>
          </div>
        )}
      </div>
    </div>
  );
}

function ProtectedRoute({ children }) {
  return isLoggedIn() ? children : <Navigate to="/login" replace />;
}

export default function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <div className="container">
        <Routes>
          <Route path="/" element={<Navigate to="/login" replace />} />
          <Route path="/login" element={<Login/>} />
          <Route path="/register" element={<Register/>} />

          <Route path="/courses" element={
            <ProtectedRoute><Courses/></ProtectedRoute>
          }/>
          <Route path="/courses/:id/materials" element={
            <ProtectedRoute><CourseMaterials/></ProtectedRoute>
          }/>

          {/* ⬇️ YENİ ROTA: Kayıtlı Kurslarım */}
          <Route path="/my-courses" element={
            <ProtectedRoute><MyCourses/></ProtectedRoute>
          }/>

          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}
