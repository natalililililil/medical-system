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

export async function login(data) {
  const response = await fetch(`${API_URL}/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
    credentials: "include"
  });

  const result = await handleApiResponse(response);
  return result;
}

export async function refreshAccessToken() {
  const response = await fetch(`${API_URL}/refresh`, {
    method: "POST",
    credentials: "include"
  });

  if (!response.ok){
    return null;
  }

  return true;
}

export async function fetchWithAuth(url, options = {}) {
  let response = await fetch(url, {
    ...options,
    credentials: "include"
  });

  if (response.status === 401) {
    const refreshed = await refreshAccessToken();

    if (!refreshed) {
      throw new Error("Session expired. Please login again.");
    }

    response = await fetch(url, {
      ...options,
      credentials: "include"
    });
  }

  return handleApiResponse(response);
}