import { useEffect, useMemo, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { internsApi } from "../services/api";
import { normalizeIntern } from "../utils/normalize";

export default function InternsList() {
  const nav = useNavigate();

  const [rows, setRows] = useState([]);
  const [total, setTotal] = useState(0);

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [status, setStatus] = useState("");

  const [typed, setTyped] = useState("");
  const [search, setSearch] = useState("");
  useEffect(() => {
    const t = setTimeout(() => setSearch(typed.trim()), 300);
    return () => clearTimeout(t);
  }, [typed]);

  const [sortBy, setSortBy] = useState("");      
  const [sortDir, setSortDir] = useState("asc"); 
  const toggleSort = (col) => {
    if (sortBy !== col) { setSortBy(col); setSortDir("asc"); }
    else setSortDir((d) => (d === "asc" ? "desc" : "asc"));
  };

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [deletingId, setDeletingId] = useState(null);

  useEffect(() => { setPage(1); }, [search, status, pageSize, sortBy, sortDir]);

  useEffect(() => {
    const run = async () => {
      setLoading(true); setError("");
      try {
        const data = await internsApi.getAll({ page, pageSize, search, status, sortBy, sortDir });

        if (Array.isArray(data?.items)) {
          const list = data.items.map(normalizeIntern);
          setRows(list);
          setTotal(Number(data.total ?? list.length));
        } else if (Array.isArray(data)) {
          const list = data.map(normalizeIntern);
          const start = (page - 1) * pageSize;
          setRows(list.slice(start, start + pageSize));
          setTotal(list.length);
        } else {
          throw new Error("Beklenmeyen cevap biçimi");
        }
      } catch (e) {
        setError(e?.message || "Bir şeyler ters gitti.");
      } finally {
        setLoading(false);
      }
    };
    run();
  }, [page, pageSize, search, status, sortBy, sortDir]);

  const totalPages = useMemo(() => Math.max(1, Math.ceil(total / pageSize)), [total, pageSize]);

  const onDelete = async (id) => {
    if (!confirm("Bu stajyeri silmek istediğine emin misin?")) return;
    setDeletingId(id);
    try {
      await internsApi.remove(id);
      setRows((r) => r.filter((x) => x.id !== id));
      setTotal((t) => Math.max(0, t - 1));
    } catch (e) {
      alert(e?.response?.data?.message || e.message || "Silme başarısız");
    } finally {
      setDeletingId(null);
    }
  };

  const Th = ({ col, children }) => (
    <th
      onClick={() => toggleSort(col)}
      style={{ cursor: "pointer", userSelect: "none", textAlign: "left", padding: 12, borderBottom: "1px solid #eee" }}
      title="Sırala"
    >
      {children}
      {sortBy === col ? (sortDir === "asc" ? " ▲" : " ▼") : ""}
    </th>
  );

  const fmt = (d) => (d ? String(d).slice(0, 10) : "—");

  return (
    <div style={{ padding: 24, fontFamily: "system-ui", maxWidth: 1100, margin: "0 auto" }}>
      <h2 style={{ fontSize: 22, fontWeight: 600, marginBottom: 12 }}>Stajyerler</h2>

      {/* Filters */}
      <div style={{ display: "flex", gap: 8, flexWrap: "wrap", marginBottom: 12 }}>
        <input
          placeholder="Ara: ad, soyad, email, TC…"
          value={typed}
          onChange={(e) => setTyped(e.target.value)}
          style={{ padding: "8px 10px", border: "1px solid #ddd", borderRadius: 8, width: 280 }}
        />
        <select value={status} onChange={(e) => setStatus(e.target.value)} style={{ padding: "8px 10px", border: "1px solid #ddd", borderRadius: 8 }}>
          <option value="">Durum (Hepsi)</option>
          <option value="Aktif">Aktif</option>
          <option value="Pasif">Pasif</option>
        </select>
        <select value={pageSize} onChange={(e) => setPageSize(Number(e.target.value))} style={{ padding: "8px 10px", border: "1px solid #ddd", borderRadius: 8 }}>
          <option value={5}>5</option>
          <option value={10}>10</option>
          <option value={20}>20</option>
        </select>
        <button onClick={() => nav("/interns/new")} style={{ marginLeft: "auto", padding: "8px 12px", borderRadius: 8, border: "1px solid #ccc" }}>
          + Yeni Stajyer
        </button>
      </div>

      {/* Table */}
      <div style={{ border: "1px solid #eee", borderRadius: 10, overflowX: "auto" }}>
        <table style={{ width: "100%", borderCollapse: "collapse", fontSize: 14 }}>
          <thead style={{ background: "#fafafa" }}>
            <tr>
              <Th col="firstName">Ad Soyad</Th>
              <Th col="identityNumber">TC Kimlik No</Th>
              <Th col="email">Email</Th>
              <Th col="phone">Telefon</Th>
              <Th col="school">Okul / Bölüm</Th>
              <Th col="startDate">Başlangıç</Th>
              <Th col="endDate">Bitiş</Th>
              <Th col="status">Durum</Th>
              <th style={{ textAlign: "right", padding: 12, borderBottom: "1px solid #eee", width: 140 }}>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr><td colSpan={9} style={{ padding: 16 }}>Yükleniyor…</td></tr>
            ) : error ? (
              <tr><td colSpan={9} style={{ padding: 16, color: "#b91c1c" }}>{error}</td></tr>
            ) : rows.length === 0 ? (
              <tr><td colSpan={9} style={{ padding: 16 }}>Kayıt bulunamadı</td></tr>
            ) : rows.map((i) => (
              <tr key={i.id} style={{ borderTop: "1px solid #f1f1f1" }}>
                <td style={{ padding: 10 }}><strong>{i.firstName} {i.lastName}</strong></td>
                <td style={{ padding: 10 }}>{i.identityNumber || "—"}</td>
                <td style={{ padding: 10 }}>{i.email || "—"}</td>
                <td style={{ padding: 10 }}>{i.phone || "—"}</td>
                <td style={{ padding: 10 }}>{(i.school || "—") + " / " + (i.department || "—")}</td>
                <td style={{ padding: 10 }}>{fmt(i.startDate)}</td>
                <td style={{ padding: 10 }}>{fmt(i.endDate)}</td>
                <td style={{ padding: 10 }}>{i.status || "—"}</td>
                <td style={{ padding: 8, textAlign: "right", whiteSpace: "nowrap" }}>
                  <button onClick={() => nav(`/interns/${i.id}/edit`)} style={{ padding: "6px 10px", borderRadius: 8, border: "1px solid #ccc", marginRight: 6 }}>
                    Düzenle
                  </button>
                  <button
                    onClick={() => onDelete(i.id)}
                    disabled={deletingId === i.id}
                    style={{ padding: "6px 10px", borderRadius: 8, border: "1px solid #f3d2d2", background: "#fff5f5", color: "#b91c1c", opacity: deletingId === i.id ? 0.6 : 1 }}
                  >
                    {deletingId === i.id ? "Siliniyor…" : "Sil"}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Pagination */}
      <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginTop: 12 }}>
        <span style={{ fontSize: 13 }}>Toplam <strong>{total}</strong> kayıt — Sayfa {page}/{totalPages}</span>
        <div style={{ display: "flex", gap: 6 }}>
          <button onClick={() => setPage((p) => Math.max(1, p - 1))} disabled={page === 1} style={{ padding: "8px 12px", borderRadius: 8, border: "1px solid #ccc", opacity: page === 1 ? 0.5 : 1 }}>
            ‹ Önceki
          </button>
          <button onClick={() => setPage((p) => Math.min(totalPages, p + 1))} disabled={page === totalPages} style={{ padding: "8px 12px", borderRadius: 8, border: "1px solid #ccc", opacity: page === totalPages ? 0.5 : 1 }}>
            Sonraki ›
          </button>
        </div>
      </div>

      <div style={{ marginTop: 16 }}>
        <Link to="/login">Login sayfasına dön</Link>
      </div>
    </div>
  );
}
