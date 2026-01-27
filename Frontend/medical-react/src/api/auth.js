const API_URL = "https://localhost:7117/api/auth";

export async function register(data) {
  const response = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (!response.ok) {
    throw new Error("Ошибка регистрации");
  }
}

export async function confirmEmail(token) {
  const response = await fetch(`${API_URL}/confirm-email`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ token })
  });

  if (!response.ok) {
    throw new Error("Ошибка подтверждения email");
  }
}
