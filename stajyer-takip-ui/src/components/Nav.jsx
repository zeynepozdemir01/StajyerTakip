import { Link, useNavigate } from "react-router-dom";

export default function Nav() {
  const nav = useNavigate();
  const authed = !!localStorage.getItem("token");

  const logout = () => {
    localStorage.removeItem("token");
    nav("/login");
  };

  return (
    <nav style={{ padding: 12, borderBottom: "1px solid #eee" }}>
      <Link to="/" style={{ marginRight: 12 }}>Stajyerler</Link>
      <Link to="/interns" style={{ marginRight: 12 }}>Liste</Link>
      <Link to="/interns" style={{ marginRight: 12 }}>Liste</Link>
      <Link to="/interns/new" style={{ marginRight: 12 }}>Yeni</Link>
      {!authed ? (
        <Link to="/login">Giriş</Link>
      ) : (
        <button onClick={logout} style={{ float: "right" }}>Çıkış</button>
      )}
    </nav>
  );
}
