import { useState, useEffect } from "react";

export default function InternForm({ initial, onSubmit, submitting }) {
  const [model, setModel] = useState({
    firstName: "",
    lastName: "",
    nationalId: "",
    email: "",
    phone: "",
    school: "",
    department: "",
    startDate: "",
    endDate: "",
    status: "Aktif",
    ...initial,
  });

  useEffect(() => {
    if (initial) setModel((m) => ({ ...m, ...initial }));
  }, [initial]);

  const set = (k) => (e) => setModel({ ...model, [k]: e.target.value });

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(model);
  };

  const row = { display: "grid", gridTemplateColumns: "140px 1fr", gap: 8, marginBottom: 10 };

  return (
    <form onSubmit={handleSubmit} style={{ maxWidth: 640 }}>
      <div style={row}><label>Ad</label><input value={model.firstName} onChange={set("firstName")} required /></div>
      <div style={row}><label>Soyad</label><input value={model.lastName} onChange={set("lastName")} required /></div>
      <div style={row}><label>TCKN</label><input value={model.nationalId} onChange={set("nationalId")} required /></div>
      <div style={row}><label>Email</label><input type="email" value={model.email} onChange={set("email")} required /></div>
      <div style={row}><label>Telefon</label><input value={model.phone ?? ""} onChange={set("phone")} /></div>
      <div style={row}><label>Okul</label><input value={model.school ?? ""} onChange={set("school")} /></div>
      <div style={row}><label>Bölüm</label><input value={model.department ?? ""} onChange={set("department")} /></div>
      <div style={row}><label>Başlangıç</label><input type="date" value={model.startDate ?? ""} onChange={set("startDate")} required /></div>
      <div style={row}><label>Bitiş</label><input type="date" value={model.endDate ?? ""} onChange={set("endDate")} /></div>
      <div style={row}>
        <label>Durum</label>
        <select value={model.status ?? "Aktif"} onChange={set("status")}>
          <option>Aktif</option>
          <option>Pasif</option>
          <option>Tamamlandı</option>
        </select>
      </div>
      <div>
        <button type="submit" disabled={submitting}>{submitting ? "Kaydediliyor…" : "Kaydet"}</button>
      </div>
    </form>
  );
}
