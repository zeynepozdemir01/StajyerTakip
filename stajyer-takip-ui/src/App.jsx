import { Routes, Route, Navigate } from "react-router-dom";
import Interns from "./pages/Interns";
import InternNew from "./pages/InternNew";
import InternEdit from "./pages/InternEdit";
import Login from "./pages/Login";
import ProtectedRoute from "./components/ProtectedRoute";

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />

      <Route element={<ProtectedRoute />}>
        <Route path="/" element={<Navigate to="/interns" replace />} />
        <Route path="/interns" element={<Interns />} />
        <Route path="/interns/new" element={<InternNew />} />
        <Route path="/interns/:id/edit" element={<InternEdit />} />
      </Route>

      <Route path="*" element={<Navigate to="/interns" replace />} />
    </Routes>
  );
}
