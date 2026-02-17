import { errorMessages } from "../constants/errorMessages";

const API_URL = "https://localhost:7117/api/auth";
window.fetchWithAuth = fetchWithAuth;

export async function handleApiResponse(response) {
  let result = null;
  try {
    result = await response.json();
  } catch (e) {
    throw new Error("Server returned invalid response");
  }

  if (!response.ok) {
    const message = errorMessages[result?.code] || result?.message || "Request failed";
    throw new Error(message);
  }

  return result;
}

export async function register(data) {
  const response = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  return handleApiResponse(response);
}

export async function confirmEmail(token) {
  const response = await fetch(`${API_URL}/confirm-email`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ token })
  });

  return handleApiResponse(response);
}

export function saveTokens({ accessToken, refreshToken }) {
  localStorage.setItem("accessToken", accessToken);
  localStorage.setItem("refreshToken", refreshToken);
}

export function getAccessToken() {
  return localStorage.getItem("accessToken");
}

export function getRefreshToken() {
  return localStorage.getItem("refreshToken");
}

export function clearTokens() {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
}

export async function login(data) {
  const response = await fetch(`${API_URL}/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  const result = await handleApiResponse(response);

  saveTokens(result);
  return result;
}

export async function refreshAccessToken() {
  const refreshToken = getRefreshToken();
  if (!refreshToken) return null;

  const response = await fetch(`${API_URL}/refresh`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ refreshToken })
  });

  if (!response.ok) {
    clearTokens();
    return null;
  }

  const result = await handleApiResponse(response);
  saveTokens(result);
  return result.accessToken;
}

export async function fetchWithAuth(url, options = {}) {
  let accessToken = getAccessToken();

  const headers = {
    "Content-Type": "application/json",
    ...(options.headers || {}),
    ...(accessToken ? { Authorization: "Bearer " + accessToken } : {})
  };

  let response = await fetch(url, { ...options, headers });

  if (response.status === 401) {
    accessToken = await refreshAccessToken();
    if (!accessToken) {
      throw new Error("Session expired. Please login again.");
    }

    const retryHeaders = {
      ...headers,
      Authorization: "Bearer " + accessToken
    };
    response = await fetch(url, { ...options, headers: retryHeaders });
  }

  return handleApiResponse(response);
}