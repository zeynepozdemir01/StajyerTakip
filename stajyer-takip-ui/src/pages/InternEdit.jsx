import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import { internsApi } from "../services/api";

export default function InternEdit() {
  const { id } = useParams();
  const nav = useNavigate();
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [err, setErr] = useState("");
  const [f, setF] = useState({
    firstName: "", lastName: "", identityNumber: "",
    email: "", phone: "", school: "", department: "",
    startDate: "", endDate: "", status: "Aktif",
  });

  useEffect(() => {
    const run = async () => {
      try {
        const d = await internsApi.getById(id);
        setF({
          firstName: d.firstName ?? "",
          lastName: d.lastName ?? "",
          identityNumber: d.identityNumber ?? "",
          email: d.email ?? "",
          phone: d.phone ?? "",
          school: d.school ?? "",
          department: d.department ?? "",
          startDate: d.startDate ? String(d.startDate).slice(0,10) : "",
          endDate: d.endDate ? String(d.endDate).slice(0,10) : "",
          status: d.status ?? "Aktif",
        });
      } catch (e) {
        setErr(e?.response?.data || e.message || "Kayıt getirilemedi");
      } finally {
        setLoading(false);
      }
    };
    run();
  }, [id]);

  const onChange = (e) => setF((s) => ({ ...s, [e.target.name]: e.target.value }));

  const onSubmit = async (e) => {
    e.preventDefault();
    setSaving(true); setErr("");
    try {
      await internsApi.update(id, {
        firstName: f.firstName,
        lastName: f.lastName,
        nationalId: f.identityNumber,       
        email: f.email,
        phone: f.phone || null,
        school: f.school || null,
        department: f.department || null,
        status: f.status || "Aktif",
        startDate: f.startDate || null,
        endDate: f.endDate || null,
      });
      nav("/interns");
    } catch (e2) {
      setErr(e2?.response?.data?.message || e2.message || "Güncelleme başarısız");
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <div style={{ padding:24 }}>Yükleniyor…</div>;

  return (
    <div style={{ padding:24, fontFamily:"system-ui", maxWidth:740, margin:"0 auto" }}>
      <h2 style={{ marginBottom:12 }}>Stajyer Düzenle</h2>
      {err && <div style={{ color:"#b91c1c", marginBottom:8 }}>{err}</div>}

      <form onSubmit={onSubmit} style={{ display:"grid", gap:10, gridTemplateColumns:"repeat(2, minmax(0,1fr))" }}>
        <label>Ad<input name="firstName" value={f.firstName} onChange={onChange} className="form-control"/></label>
        <label>Soyad<input name="lastName" value={f.lastName} onChange={onChange} className="form-control"/></label>
        <label>TC Kimlik<input name="identityNumber" value={f.identityNumber} onChange={onChange} className="form-control"/></label>
        <label>Email<input name="email" type="email" value={f.email} onChange={onChange} className="form-control"/></label>
        <label>Telefon<input name="phone" value={f.phone} onChange={onChange} className="form-control"/></label>
        <label>Okul<input name="school" value={f.school} onChange={onChange} className="form-control"/></label>
        <label>Bölüm<input name="department" value={f.department} onChange={onChange} className="form-control"/></label>
        <label>Başlangıç<input name="startDate" type="date" value={f.startDate} onChange={onChange} className="form-control"/></label>
        <label>Bitiş<input name="endDate" type="date" value={f.endDate} onChange={onChange} className="form-control"/></label>
        <label>Durum
          <select name="status" value={f.status} onChange={onChange} className="form-control">
            <option value="Aktif">Aktif</option>
            <option value="Pasif">Pasif</option>
          </select>
        </label>

        <div style={{ gridColumn:"1 / -1", display:"flex", gap:8, marginTop:8 }}>
          <button type="submit" disabled={saving} className="btn btn-primary">
            {saving ? "Kaydediliyor…" : "Kaydet"}
          </button>
          <Link to="/interns" className="btn btn-secondary">İptal</Link>
        </div>
      </form>
    </div>
  );
}
