const API_URL = "https://localhost:7117/api/auth";
window.fetchWithAuth = fetchWithAuth;

export async function register(data) {
  const response = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  const result = await response.json();

  if (!response.ok) {
    if (result.message) {
      throw new Error(result.message);
    }

    if (result.errors) {
      const firstError = Object.values(result.errors)[0][0];
      throw new Error(firstError);
    }

    throw new Error("Registration failed");
  }
}

export async function confirmEmail(token) {
  const response = await fetch(`${API_URL}/confirm-email`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ token })
  });

  if (!response.ok) {
    throw new Error("Email confirmation error");
  }
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

  const result = await response.json();

  if (!response.ok) {
    throw new Error(result.message || "Login failed");
  }

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

  const result = await response.json();
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

  let result = null;
  try {
    result = await response.json();
  } catch (e) {
  }

  if (!response.ok) {
    const message = result?.message || "Request failed";
    throw new Error(message);
  }

  return result;
}