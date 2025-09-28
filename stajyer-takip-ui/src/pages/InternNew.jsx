import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import api from "../services/api";
import InternForm from "./InternForm";

export default function InternNew() {
  const [err, setErr] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const nav = useNavigate();

  const submit = async (m) => {
    setErr("");
    setSubmitting(true);
    try {
      const payload = {
        firstName: m.firstName,
        lastName: m.lastName,
        nationalId: m.nationalId,
        email: m.email,
        phone: m.phone || null,
        school: m.school || null,
        department: m.department || null,
        startDate: m.startDate, 
        endDate: m.endDate || null,
        status: m.status || "Aktif",
      };
      await api.post("/Interns", payload);
      nav("/interns");
    } catch (e) {
      setErr(e?.response?.data?.title || "Kayıt başarısız.");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div style={{ maxWidth: 720, margin: "24px auto" }}>
      <h2>Yeni Stajyer</h2>
      {err && <p style={{ color: "crimson" }}>{err}</p>}
      <InternForm onSubmit={submit} submitting={submitting} />
      <p style={{ marginTop: 12 }}><Link to="/interns">Listeye dön</Link></p>
    </div>
  );
}
