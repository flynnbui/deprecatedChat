import axios from "axios";
import { getRefreshToken, setJwtToken, setRefreshToken, getJwtToken } from "../pages/auth/auth";

const baseUrl = "http://localhost:5000/api";

const config = {
  baseURL: baseUrl,
  timeout: 30000, 
};

const api = axios.create(config);

const refreshToken = async () => {
  const storedRefreshToken = getRefreshToken();
  const storedToken = getJwtToken();
  if (!storedRefreshToken) return null;

  try {
    const response = await axios.post(`${baseUrl}/auth/refreshtoken`, { Token: storedToken, RefreshToken: storedRefreshToken });
    const { token, refreshToken } = response.data;

    setJwtToken(token);
    setRefreshToken(refreshToken);
    return token;
  } catch (error) {
    console.error('Refresh token request failed:', error);
    return null;
  }
};

const handleBefore = (config) => {
  const token = getJwtToken();
  if (token) {
    config.headers["Authorization"] = `Bearer ${token}`;
  }
  return config;
};

const handleError = async (error) => {
  const originalRequest = error.config;

  if (error.response && error.response.status === 401 && !originalRequest._retry) {
    originalRequest._retry = true;
    const newToken = await refreshToken();
    if (newToken) {
      originalRequest.headers['Authorization'] = `Bearer ${newToken}`;
      return api(originalRequest);
    }
  }
  return Promise.reject(error);
};

// Request interceptor
api.interceptors.request.use(handleBefore, error => Promise.reject(error));

// Response interceptor
api.interceptors.response.use(
  response => response,
  handleError
);

export default api;
