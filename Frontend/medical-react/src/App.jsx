import { useState } from "react"; // обязательно!
import { BrowserRouter, Routes, Route } from "react-router-dom";
import RegisterModal from "./components/RegisterModal";
import ConfirmEmail from "./components/ConfirmEmail";

function Home() {
  const [showRegister, setShowRegister] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);

  return (
    <div style={{ padding: "20px" }}>
      <button onClick={() => setShowRegister(true)}>Зарегистрироваться</button>

      {showRegister && (
        <RegisterModal
          onSuccess={() => {
            setShowRegister(false);
            setShowConfirm(true);
          }}
        />
      )}

      {showConfirm && (
        <div style={{ marginTop: "20px" }}>
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
