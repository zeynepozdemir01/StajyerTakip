import axios from "axios";
import { auth } from "./auth";

export const http = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || "https://localhost:7007",
  timeout: 15000,
});

http.interceptors.request.use((cfg) => {
  const t = auth.getToken();
  if (t) cfg.headers.Authorization = `Bearer ${t}`;
  return cfg;
});
