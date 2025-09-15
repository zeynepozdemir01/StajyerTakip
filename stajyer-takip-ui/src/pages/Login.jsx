import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { http } from "../http";
import { auth } from "../auth";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [err, setErr] = useState("");
  const [loading, setLoading] = useState(false);
  const nav = useNavigate();

  const onSubmit = async (e) => {
    e.preventDefault();
    setErr(""); setLoading(true);
    try {
      const { data } = await http.post("/api/auth/login", { email, password });
      const token = data?.token || data; 
      if (!token) throw new Error("Token alınamadı");
      auth.setToken(token);
      nav("/", { replace: true });
    } catch (ex) {
      setErr(ex?.response?.data?.message || ex.message || "Giriş başarısız");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ padding: 24, fontFamily: "system-ui" }}>
      <h2>Giriş</h2>
      <form onSubmit={onSubmit} style={{ display: "grid", gap: 8, maxWidth: 320 }}>
        <input placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} autoComplete="username" />
        <input placeholder="Şifre" type="password" value={password} onChange={(e) => setPassword(e.target.value)} autoComplete="current-password" />
        <button type="submit" disabled={loading}>{loading ? "Gönderiliyor..." : "Giriş yap"}</button>
      </form>
      {err && <p style={{ color: "crimson", marginTop: 8 }}>{err}</p>}
    </div>
  );
}
