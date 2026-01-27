import { useEffect, useState } from "react";
import { confirmEmail } from "../api/auth";
import "../App.css";

export default function ConfirmEmail() {
  const [message, setMessage] = useState("Подтверждаем email...");

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    const token = params.get("token");

    if (!token) {
      setMessage("Токен не найден");
      return;
    }

    confirmEmail(token)
      .then(() => setMessage("Email успешно подтвержден!"))
      .catch(() => setMessage("Ссылка недействительна или токен истёк"));
  }, []);

  return (
    <div className="modal">
      <h2>Подтверждение email</h2>
      <p>{message}</p>
    </div>
  );
}
