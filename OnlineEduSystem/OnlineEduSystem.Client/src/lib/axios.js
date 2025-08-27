import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  const url = (config.url || "").toLowerCase();
  const isAuthEndpoint = url.includes("/api/auth/");

  const validToken = token && token !== "null" && token !== "undefined";
  if (validToken && !isAuthEndpoint) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
