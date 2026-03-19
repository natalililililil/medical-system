import { useState, useEffect } from "react";
import { logout, checkAuth } from "./api/auth";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import RegisterModal from "./components/RegisterModal";
import LoginModal from "./components/LoginModal";
import ConfirmEmail from "./components/ConfirmEmail";
import "./App.css";

function Home() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  const [showRegister, setShowRegister] = useState(false);
  const [showLogin, setShowLogin] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);

  useEffect(() => {
    async function initAuth() {
      try {
        await checkAuth(); 
        setIsAuthenticated(true);
      } catch (e) {
        setIsAuthenticated(false);
      } finally {
        setLoading(false);
      }
    }
    initAuth();
  }, []);

  async function handleLogout() {
    try {
      await logout();
      setIsAuthenticated(false);
    } catch (e) {
      console.error(e);
    }}

  return (
    <div>
      <div className="header">
        <div className="project-title">Medical System</div>

        <div>
          {!isAuthenticated ? (
            <>
              <button className="register-button" onClick={() => setShowLogin(true)}>
                Sign in
              </button>
              <button className="register-button" onClick={() => setShowRegister(true)}>
                Sign up
              </button>
            </>
          ) : (
            <button className="register-button" onClick={handleLogout}>
              Sign out
            </button>
          )}
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
        <LoginModal 
        onClose={() => setShowLogin(false)} 
        onLoginSuccess={() => setIsAuthenticated(true)}
        />
      )}

      {showConfirm && (
        <div className="modal">
          <button className="close-button" onClick={() => setShowConfirm(false)}>×</button>
          <h2>We have sent a confirmation email.</h2>
          <p>Please click the link in the email to complete your registration.</p>
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