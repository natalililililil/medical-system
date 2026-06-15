import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import Header from "./components/Header";
import ConfirmEmail from "./components/ConfirmEmail";
import DoctorsPage from "./pages/DoctorsPage";
import ProfilePage from "./pages/ProfilePage";
import UsersListPage from "./pages/UsersListPage";
import "./App.css";

function Home() {
  return (
    <div className="home-container">
      <section className="hero-section">
        <div className="hero-content">
          <span className="hero-subtitle">Connecting You to Care</span>
          <h1>Medical System</h1>
          <p>
            Your portal to personalized medical information and professional scheduling. 
            Choose a doctor and make an appointment in just a few clicks.
          </p>
          <div className="hero-buttons">
            <Link to="/doctors" className="btn-primary">Explore List of Doctors</Link>
            <Link to="/" className="btn-secondary">Make an appointment</Link>
          </div>
        </div>
        <div className="hero-image">
          <img src="https://img.freepik.com/free-vector/doctors-concept-illustration_114360-1515.jpg" alt="Medical Team" />
        </div>
      </section>

      <section className="info-blocks">
        <div className="info-card">
          <h3>Our Vision</h3>
          <p>To redefine patient care through technological innovation and accessibility.</p>
        </div>
        <div className="info-card">
          <h3>Core Values</h3>
          <p>Integrity, Compassion, and Excellence in every medical interaction.</p>
        </div>
        <div className="info-card">
          <h3>Why Choose Us</h3>
          <p>Experienced specialists and a patient-centric approach to healthcare.</p>
        </div>
      </section>

      <section className="cta-section">
        <h2>Contact Imformation</h2>
        
        <div className="contact-info-simple">
          <p>📍 123 Healthcare Ave, London, UK</p>
          <p>📞 +44 20 7946 0000</p>
          <p>📧 support@medsystem.com</p>
        </div>
      </section>
    </div>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <Header /> 
      <main>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/confirm-email" element={<ConfirmEmail />} />
          <Route path="/doctors" element={<DoctorsPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/profile/:targetRole/:targetId" element={<ProfilePage />} />
          <Route path="/users" element={<UsersListPage />} />
        </Routes>
      </main>
    </BrowserRouter>
  );
}