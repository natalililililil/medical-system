import { useState, useEffect, useRef } from "react";
import { Link, useNavigate } from "react-router-dom";
import { logout, checkAuth, fetchWithAuth } from "../api/auth";
import RegisterModal from "./RegisterModal";
import LoginModal from "./LoginModal";

export default function Header() {
  const [user, setUser] = useState(null);
  const [showRegister, setShowRegister] = useState(false);
  const [showLogin, setShowLogin] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [showDropdown, setShowDropdown] = useState(false);

  const navigate = useNavigate();
  const dropdownRef = useRef(null);

  useEffect(() => {
    async function initAuth() {
      try {
        const authData = await checkAuth();

        const rolePath = authData.role.toLowerCase();
        const profileData = await fetchWithAuth(`https://localhost:7260/api/profiles/${rolePath}/me`);
        
        setUser({ ...authData, photoUrl: profileData.photoUrl });
      } catch (e) {
        setUser(null);
      }
    }
    initAuth();
  }, []);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  async function handleLogout() {
    try {
      await logout();
      setUser(null);
      navigate("/");
      window.location.reload();
    } catch (e) {
      console.error(e);
    }
  }

  const getProfileLink = () => {
    return "/profile";
  };

  return (
    <>
      <div className="header">
        <div className="header-content">
          <Link to="/" className="project-title" style={{ textDecoration: 'none', color: 'inherit' }}>
          Medical System
        </Link>

        <div>
          {!user ? (
            <>
              <button className="register-button" onClick={() => setShowLogin(true)}>
                Sign in
              </button>
              <button className="register-button" onClick={() => setShowRegister(true)}>
                Sign up
              </button>
            </>
          ) : (
            <div className="user-profile-section" ref={dropdownRef}>
              <div className="avatar-wrapper" onClick={() => setShowDropdown(!showDropdown)}>
                <img 
                  src={user.photoUrl || "https://cdn-icons-png.flaticon.com/512/149/149071.png"} 
                  alt="Profile" 
                  className="header-avatar"
                />
              </div>

              {showDropdown && (
                <div className="profile-style-menu">
                  <div className="menu-header">
                    <span className="signed-in-as">Signed in as</span>
                    <span className="username">{user.role}</span>
                  </div>
                  <div className="menu-divider"></div>
                  <Link to={getProfileLink()} onClick={() => setShowDropdown(false)}>
                    My profile
                  </Link>
                  {user.role === 'Receptionist' && (
                    <Link to="/users" onClick={() => setShowDropdown(false)}>
                      Manage All Users
                    </Link>
                  )}
                  <div className="menu-divider"></div>
                  <button onClick={handleLogout} className="logout-button">Sign out</button>
                </div>
              )}
            </div>
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
          onLoginSuccess={async () => {
            const authData = await checkAuth();
            setUser(authData);
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