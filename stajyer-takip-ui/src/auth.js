export const auth = {
    getToken() {
      return localStorage.getItem("token");
    },
    setToken(t) {
      localStorage.setItem("token", t);
    },
    clear() {
      localStorage.removeItem("token");
    },
    isAuthed() {
      return !!localStorage.getItem("token");
    }
  };
  