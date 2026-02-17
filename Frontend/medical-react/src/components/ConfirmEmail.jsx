import { useEffect, useState } from "react";
import { confirmEmail } from "../api/auth";
import { errorMessages } from "../constants/errorMessages";
import "../App.css";

export default function ConfirmEmail() {
  const [message, setMessage] = useState("Confirming email...");

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    const token = params.get("token");

    if (!token) {
      setMessage("Token not found");
      return;
    }

    confirmEmail(token)
      .then(() => setMessage("Email successfully confirmed!"))
      .catch((err) => setMessage(err.message || errorMessages.INVALID_EMAIL_CONFIRMATION));
  }, []);

  return (
    <div className="modal">
      <h2>Email Confirmation</h2>
      <p>{message}</p>
    </div>
  );
}