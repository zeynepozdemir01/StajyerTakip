import axios from "axios";

const BASE = (import.meta.env.VITE_API_URL || "https://localhost:7007/api").replace(/\/$/, "");
const api = axios.create({ baseURL: BASE });

api.interceptors.request.use(cfg => {
  const t = localStorage.getItem("token");
  if (t) cfg.headers.Authorization = `Bearer ${t}`;
  return cfg;
});

let isRefreshing = false;
let queue = [];

function drainQueue(err, newToken) {
  queue.forEach(p => (err ? p.reject(err) : p.resolve(newToken)));
  queue = [];
}

api.interceptors.response.use(
  r => r,
  async (err) => {
    const original = err.config;
    if (err?.response?.status === 401 && !original._retry) {
      original._retry = true;
      const rt = localStorage.getItem("refreshToken");
      if (!rt) {
        localStorage.removeItem("token");
        window.location.href = "/login";
        return Promise.reject(err);
      }

      if (!isRefreshing) {
        isRefreshing = true;
        try {
          const { data } = await axios.post(`${BASE}/auth/refresh`, { refreshToken: rt });
          localStorage.setItem("token", data.accessToken);
          localStorage.setItem("refreshToken", data.refreshToken);
          drainQueue(null, data.accessToken);
          original.headers.Authorization = `Bearer ${data.accessToken}`;
          return api(original);
        } catch (e) {
          drainQueue(e, null);
          localStorage.removeItem("token");
          localStorage.removeItem("refreshToken");
          window.location.href = "/login";
          return Promise.reject(e);
        } finally {
          isRefreshing = false;
        }
      }

      return new Promise((resolve, reject) => {
        queue.push({
          resolve: (newToken) => {
            original.headers.Authorization = `Bearer ${newToken}`;
            resolve(api(original));
          },
          reject
        });
      });
    }
    return Promise.reject(err);
  }
);

export const authApi = {
  async login(username, password) {
    const { data } = await api.post("/auth/login", { username, password });
    localStorage.setItem("token", data.accessToken);
    localStorage.setItem("refreshToken", data.refreshToken);
    return data;
  },
  async logout() {
    const rt = localStorage.getItem("refreshToken");
    try { await api.post("/auth/logout", { refreshToken: rt }); } catch {}
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
  }
};

export default api;
