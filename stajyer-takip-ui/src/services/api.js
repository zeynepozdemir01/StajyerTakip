import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || "https://localhost:7007/api",
});
api.interceptors.request.use((cfg) => {
  const token = localStorage.getItem("token");
  if (token) cfg.headers.Authorization = `Bearer ${token}`;
  return cfg;
});

export const internsApi = {
  async getAll({ page=1, pageSize=10, search="", status="", sortBy="", sortDir="asc" } = {}) {
    const r = await api.get("/interns", {
      params: {
        page, pageSize,
        q: search || undefined,
        status: status || undefined,
        sortField: sortBy || undefined,
        sortOrder: sortDir || undefined,
      },
    });
    return r.data;
  },
  async getById(id) {                       
    const r = await api.get(`/interns/${id}`);
    return r.data;
  },
  async create(payload) {
    const r = await api.post("/interns", payload);
    return r.data;
  },
  async update(id, payload) {           
    await api.put(`/interns/${id}`, payload);
  },
  async remove(id) {
    await api.delete(`/interns/${id}`);
  },
};

export default api;
