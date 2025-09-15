import { useState, useEffect } from "react";

const toInputDate = (v) => {
  if (!v) return "";
  const d = new Date(v);
  return new Date(d.getTime() - d.getTimezoneOffset()*60000)
    .toISOString()
    .slice(0,10);
};

export default function InternForm({ initial, onSubmit, submitText = "Kaydet" }) {
  const [m, setM] = useState({
    firstName: "", lastName: "", nationalId: "",
    email: "", phone: "", school: "", department: "",
    startDate: "", endDate: "", status: "Aktif",
    ...initial,
  });

  useEffect(()=>{ if(initial) setM((x)=>({ ...x, ...initial })); }, [initial]);

  const change = (e) => setM({ ...m, [e.target.name]: e.target.value });

  const handle = (e) => {
    e.preventDefault();
    const payload = {
      ...m,
      startDate: m.startDate || null,
      endDate: m.endDate || null,
    };
    onSubmit?.(payload);
  };

  return (
    <form onSubmit={handle} style={{ maxWidth: 600 }}>
      <div style={{display:"grid",gridTemplateColumns:"1fr 1fr",gap:8}}>
        <input name="firstName"  placeholder="Ad"        value={m.firstName}  onChange={change} required />
        <input name="lastName"   placeholder="Soyad"     value={m.lastName}   onChange={change} required />
        <input name="nationalId" placeholder="TC Kimlik" value={m.nationalId} onChange={change} required />
        <input name="email"      placeholder="Email"     value={m.email}      onChange={change} required />
        <input name="phone"      placeholder="Telefon"   value={m.phone}      onChange={change} />
        <input name="school"     placeholder="Okul"      value={m.school}     onChange={change} />
        <input name="department" placeholder="Bölüm"     value={m.department} onChange={change} />
        <select name="status" value={m.status} onChange={change}>
          <option value="Aktif">Aktif</option>
          <option value="Pasif">Pasif</option>
        </select>
        <input type="date" name="startDate" value={toInputDate(m.startDate)} onChange={change} />
        <input type="date" name="endDate"   value={toInputDate(m.endDate)}   onChange={change} />
      </div>
      <button type="submit" style={{marginTop:12}}>{submitText}</button>
    </form>
  );
}
