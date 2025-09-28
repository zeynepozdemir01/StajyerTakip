import { Routes, Route, Navigate } from "react-router-dom";
import ProtectedRoute from "./components/ProtectedRoute";
import InternsPage from "./pages/Interns";     // liste sayfan
import InternEdit from "./pages/InternEdit";  // düzenleme sayfan (ekledik)
import Login from "./pages/Login";            // giriş sayfan

export default function App() {
  return (
    <Routes>
      {/* kök adresi internslere yönlendir */}
      <Route path="/" element={<Navigate to="/interns" replace />} />

      {/* login herkese açık */}
      <Route path="/login" element={<Login />} />

      {/* korumalı sayfalar */}
      <Route
        path="/interns"
        element={
          <ProtectedRoute>
            <InternsPage />
          </ProtectedRoute>
        }
      />

      {/* düzenleme sayfası */}
      <Route
        path="/interns/:id"
        element={
          <ProtectedRoute>
            <InternEdit />
          </ProtectedRoute>
        }
      />

      {/* yakalanmayan her şey internslere */}
      <Route path="*" element={<Navigate to="/interns" replace />} />
    </Routes>
  );
}
