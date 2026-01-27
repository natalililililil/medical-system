import { useState } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import RegisterModal from "./components/RegisterModal";
import ConfirmEmail from "./components/ConfirmEmail";
import "./App.css";

function Home() {
  const [showRegister, setShowRegister] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);

  return (
    <div>
      <div className="header">
        <div className="project-title">Medical System</div>
        <button className="register-button" onClick={() => setShowRegister(true)}>
          Зарегистрироваться
        </button>
      </div>

      {showRegister && (
        <RegisterModal
          onClose={() => setShowRegister(false)}
          onSuccess={() => {
            setShowRegister(false);
            setShowConfirm(true);
          }}
        />
      )}

      {showConfirm && (
        <div className="modal">
          <button className="close-button" onClick={() => setShowConfirm(false)}>×</button>
          <h2>Мы отправили письмо на вашу почту.</h2>
          <p>Перейдите по ссылке из письма, чтобы подтвердить регистрацию.</p>
        </div>
      )}
    </div>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/confirm-email" element={<ConfirmEmail />} />
      </Routes>
    </BrowserRouter>
  );
}