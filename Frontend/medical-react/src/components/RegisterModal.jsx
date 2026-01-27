import { useState } from "react";
import { register } from "../api/auth";
import "../App.css";

export default function RegisterModal({ onSuccess, onClose }) {
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
    <div className="modal">
      <button className="close-button" onClick={onClose}>×</button>
      <form onSubmit={handleSubmit}>
        <h2>Регистрация</h2>
        <input placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
        <input type="password" placeholder="Пароль" value={password} onChange={e => setPassword(e.target.value)} />
        <input type="password" placeholder="Повторите пароль" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} />
        {error && <p className="error-message">{error}</p>}
        <button type="submit">Зарегистрироваться</button>
      </form>
    </div>
  );
}
