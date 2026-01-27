import { useEffect, useState } from "react";
import { confirmEmail } from "../api/auth";

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
    <div style={{
      position: "fixed",
      top: "50%",
      left: "50%",
      transform: "translate(-50%, -50%)",
      background: "white",
      padding: "20px",
      border: "1px solid #ccc"
    }}>
      <h2>Подтверждение email</h2>
      <p>{message}</p>
    </div>
  );
}
