import { Navigate } from "react-router-dom";

export default function ProtectedRoute({ children }) {
  const hasToken = !!localStorage.getItem("token"); // TEK ANAHTAR
  return hasToken ? children : <Navigate to="/login" replace />;
}
