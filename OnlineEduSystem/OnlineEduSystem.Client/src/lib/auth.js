// src/lib/auth.js

// --- küçük yardımcı: JWT payload decode (url-safe base64) ---
function decodePayloadSegment(seg) {
  try {
    seg = seg.replace(/-/g, "+").replace(/_/g, "/");
    return JSON.parse(atob(seg));
  } catch {
    return null;
  }
}

// --- event sistemi: auth değişince navbar vs. yeniden render olsun ---
const authListeners = new Set();
function emitAuthChange() {
  authListeners.forEach(fn => { try { fn(); } catch {} });
}
export function onAuthChange(cb) {
  authListeners.add(cb);
  return () => authListeners.delete(cb);
}

// --- token helpers ---
export function getToken() {
  const t = localStorage.getItem("token");
  return t && t !== "null" && t !== "undefined" ? t : null;
}
export function setToken(t) {
  localStorage.setItem("token", t);
  emitAuthChange();
}
export function clearToken() {
  localStorage.removeItem("token");
  emitAuthChange();
}
export { clearToken as logout };

// --- kullanıcı bilgisi / rol ---
export function getUserFromToken() {
  const token = getToken();
  if (!token) return null;
  const parts = token.split(".");
  if (parts.length !== 3) return null;
  const payload = decodePayloadSegment(parts[1]);
  if (!payload) return null;

  const id =
    payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] ||
    payload.sub || payload.nameid;

  const userName =
    payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ||
    payload.unique_name || payload.name;

  const role =
    payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
    payload.role;

  return { id, userName, role };
}

export function isTokenValid() {
  const token = getToken();
  if (!token) return false;
  const parts = token.split(".");
  if (parts.length !== 3) return false;
  const payload = decodePayloadSegment(parts[1]);
  if (!payload) return false;
  if (typeof payload.exp === "number") return payload.exp * 1000 > Date.now();
  return !!payload;
}

export const isLoggedIn   = () => !!getUserFromToken();
export const isAdmin      = (u = getUserFromToken()) => u?.role === "Admin";
export const isInstructor = (u = getUserFromToken()) => u?.role === "Instructor";
export const isStudent    = (u = getUserFromToken()) => u?.role === "Student";
export const getUser      = getUserFromToken; // backward compat
