import { useState } from "react";
import { register } from "../api/auth";

export default function RegisterModal({ onSuccess }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");

  async function handleSubmit(e) {
    e.preventDefault();

    if (password !== confirmPassword) {
      setError("Пароли не совпадают");
      return;
    }

    try {
      await register({ email, password, confirmPassword });
      onSuccess();
    } catch {
      setError("Ошибка регистрации");
    }
  }

  return (
    <div style={{
      position: "fixed",
      top: "50%",
      left: "50%",
      transform: "translate(-50%, -50%)",
      background: "white",
      padding: "20px",
      border: "1px solid #ccc"
    }}>
      <form onSubmit={handleSubmit}>
        <h2>Регистрация</h2>

        <input
          placeholder="Email"
          value={email}
          onChange={e => setEmail(e.target.value)}
        />

        <input
          type="password"
          placeholder="Пароль"
          value={password}
          onChange={e => setPassword(e.target.value)}
        />

        <input
          type="password"
          placeholder="Повторите пароль"
          value={confirmPassword}
          onChange={e => setConfirmPassword(e.target.value)}
        />

        {error && <p style={{ color: "red" }}>{error}</p>}

        <button type="submit">Зарегистрироваться</button>
      </form>
    </div>
  );
}
