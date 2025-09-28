import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

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
      const { data } = await api.post("/Auth/login", { email, password });
      const token = typeof data === "string" ? data : data?.accessToken || data?.token;
      if (!token) throw new Error("Access token alınamadı");

      localStorage.setItem("token", token);
      if (data?.refreshToken) localStorage.setItem("refreshToken", data.refreshToken);

      nav("/interns", { replace: true });
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
        <input placeholder="Email" value={email} onChange={e=>setEmail(e.target.value)} />
        <input placeholder="Şifre" type="password" value={password} onChange={e=>setPassword(e.target.value)} />
        <button type="submit" disabled={loading}>{loading ? "Gönderiliyor..." : "Giriş yap"}</button>
      </form>
      {err && <p style={{ color: "crimson" }}>{err}</p>}
    </div>
  );
}
