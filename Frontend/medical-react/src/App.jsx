import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import Header from "./components/Header";
import ConfirmEmail from "./components/ConfirmEmail";
import DoctorsPage from "./pages/DoctorsPage";
import ProfilePage from "./pages/ProfilePage";
import "./App.css";

function Home() {
  return (
    <div className="home-container">

      <div style={{ marginTop: '30px' }}>
        <Link to="/doctors" className="doctors-link-button">
           List of doctors
        </Link>
      </div>
      <h1>Welcome to Medical System</h1>      
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
        </Routes>
      </main>
    </BrowserRouter>
  );
}