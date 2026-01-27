const API_URL = "https://localhost:7117/api/auth";

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
