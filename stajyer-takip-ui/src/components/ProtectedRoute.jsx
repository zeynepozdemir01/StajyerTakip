import { Outlet, Navigate } from "react-router-dom";
import { auth } from "../auth";

export default function ProtectedRoute() {
  const token = auth.getToken?.() || localStorage.getItem("token");
  return token ? <Outlet /> : <Navigate to="/login" replace />;
}
