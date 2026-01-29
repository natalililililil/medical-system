import { useState } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import RegisterModal from "./components/RegisterModal";
import LoginModal from "./components/LoginModal";
import ConfirmEmail from "./components/ConfirmEmail";
import "./App.css";

function Home() {
  const [showRegister, setShowRegister] = useState(false);
  const [showLogin, setShowLogin] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);

  return (
    <div>
      <div className="header">
        <div className="project-title">Medical System</div>

        <div>
          <button className="register-button" onClick={() => setShowLogin(true)}>
            Войти
          </button>
          <button className="register-button" onClick={() => setShowRegister(true)}>
            Зарегистрироваться
          </button>
        </div>
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

      {showLogin && (
        <LoginModal onClose={() => setShowLogin(false)} />
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