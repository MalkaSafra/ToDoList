import axios from 'axios';

// Config Defaults - הגדרת baseURL כברירת מחדל
axios.defaults.baseURL = process.env.REACT_APP_API_URL;
axios.defaults.headers.common['Content-Type'] = 'application/json';

// Interceptor לתפיסת שגיאות
axios.interceptors.response.use(
  (response) => {
    // אם התגובה תקינה, פשוט מחזירים אותה
    return response;
  },
  (error) => {
    // תפיסת שגיאות ורישום ללוג
    console.error('❌ Axios Error:', {
      status: error.response?.status,
      statusText: error.response?.statusText,
      message: error.message,
      url: error.config?.url,
      data: error.response?.data
    });

    // אם זו שגיאה 401 (Unauthorized), מעבירים לדף לוגין
    if (error.response?.status === 401) {
      console.warn('🔒 Unauthorized! Redirecting to login...');
      // מחיקת הטוקן
      localStorage.removeItem('token');
      // הפניה לדף לוגין
      window.location.href = '/login';
    }

    // ממשיכים לזרוק את השגיאה כדי שהקוד יוכל לטפל בה
    return Promise.reject(error);
  }
);

// Request Interceptor - הוספת JWT Token לכל בקשה
axios.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default {
  // Auth Methods
  register: async (username, password) => {
    const result = await axios.post('/auth/register', { username, password });
    return result.data;
  },

  login: async (username, password) => {
    const result = await axios.post('/auth/login', { username, password });
    if (result.data.token) {
      localStorage.setItem('token', result.data.token);
    }
    return result.data;
  },

  logout: () => {
    localStorage.removeItem('token');
  },

  isAuthenticated: () => {
    return !!localStorage.getItem('token');
  },

  // Tasks Methods
  getTasks: async () => {
    const result = await axios.get('/items');
    return result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name);
    const result = await axios.post('/items', { nameI: name, isComplete: false });
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete });
    const result = await axios.put(`/items/${id}`, { isComplete });
    return result.data;
  },

  deleteTask: async (id) => {
    console.log('deleteTask', id);
    const result = await axios.delete(`/items/${id}`);
    return result.data;
  }
};