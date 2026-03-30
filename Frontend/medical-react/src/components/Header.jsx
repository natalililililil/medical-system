import { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { logout, checkAuth } from "../api/auth";
import RegisterModal from "./RegisterModal";
import LoginModal from "./LoginModal";

export default function Header() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
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
      }
    }
    initAuth();
  }, []);

  async function handleLogout() {
    try {
      await logout();
      setIsAuthenticated(true); 
      window.location.reload();
    } catch (e) {
      console.error(e);
    }
  }

  return (
    <>
      <div className="header">
        <div className="header-content">
          <Link to="/" className="project-title" style={{ textDecoration: 'none', color: 'inherit' }}>
          Medical System
        </Link>

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
          onLoginSuccess={() => {
            setIsAuthenticated(true);
            setShowLogin(false);
          }} 
        />
      )}

      {showConfirm && (
        <div className="modal">
          <button className="close-button" onClick={() => setShowConfirm(false)}>×</button>
          <h2>We have sent a confirmation email.</h2>
          <p>Please click the link in the email to complete your registration.</p>
        </div>
      )}
    </>
  );
}