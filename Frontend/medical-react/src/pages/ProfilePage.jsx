import { useState, useEffect } from "react";
import { fetchWithAuth } from "../api/auth";

export default function ProfilePage() {
  const [user, setUser] = useState(null);
  const [profile, setProfile] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [formData, setFormData] = useState({});

  useEffect(() => {
    async function loadData() {
      try {
        const authData = await fetchWithAuth("https://localhost:7260/api/auth/me");
        setUser(authData);

        const rolePath = authData.role.toLowerCase();
        const data = await fetchWithAuth(`https://localhost:7260/api/profiles/${rolePath}s/me`);

        if (data.status === "AtWork") data.status = 1;
          else if (data.status === "OnVacation") data.status = 2;
          else if (data.status === "Sick") data.status = 3;

        setProfile(data);
        setFormData(data);
      } catch (e) {
        console.error("Load failed", e);
      }
    }
    loadData();
  }, []);

  const handleSave = async () => {
    try {

      const dataToSend = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      middleName: formData.middleName,
      dateOfBirth: formData.dateOfBirth,
      careerStartYear: parseInt(formData.careerStartYear || 0),
      photoUrl: formData.photoUrl,
      specializationName: formData.specializationName, 
      officeId: formData.officeId,
      status: parseInt(formData.status || 1)
      };

      const rolePath = user.role.toLowerCase();
      await fetchWithAuth(`https://localhost:7260/api/profiles/${rolePath}s/update`, {
        method: 'PATCH',
        headers: {
        'Content-Type': 'application/json',
        },
        body: JSON.stringify(dataToSend)
      });

      setProfile(dataToSend);
      setIsEditing(false);
      alert("Changes saved!");
    } catch (e) {
      alert("Error: " + e.message);
    }
  };

  if (!profile) return <div className="loading-state">Loading...</div>;

  return (
    <div className="main-content-wrapper">
      <div className="profile-container">
        <div className="profile-content">
          
          <div className="profile-aside">
            <div className="avatar-wrapper-in-profile">
              <img src={profile.photoUrl || "/default-avatar.png"} alt="Avatar" className="main-profile-img" />
              {isEditing && <button className="change-photo-link">Update Photo</button>}
            </div>
          </div>

          <div className="profile-main">
            <h2 className="profile-title">{user.role} Profile</h2>
            
            <div className="info-grid">
              <div className="info-item">
                <label>First Name</label>
                <input 
                  disabled={!isEditing} 
                  value={formData.firstName || ""} 
                  onChange={e => setFormData({...formData, firstName: e.target.value})}
                />
              </div>

              <div className="info-item">
                <label>Last Name</label>
                <input 
                  disabled={!isEditing} 
                  value={formData.lastName || ""} 
                  onChange={e => setFormData({...formData, lastName: e.target.value})}
                />
              </div>
              
              <div className="info-item">
                <label>Middle Name</label>
                <input 
                  disabled={!isEditing} 
                  value={formData.middleName || ""} 
                  onChange={e => setFormData({...formData, middleName: e.target.value})}
                />
              </div>

              <div className="info-item">
                <label>Date of Birth</label>
                <input 
                  type="date"
                  disabled={!isEditing} 
                  value={formData.dateOfBirth?.split('T')[0] || ""} 
                  onChange={e => setFormData({...formData, dateOfBirth: e.target.value})}
                />
              </div>

              {(user.role === 'Doctor' || user.role === 'Receptionist') && (
                <div className="info-item">
                  <label>Work Status</label>
                  <select 
                    disabled={!isEditing} 
                    value={formData.status || 1} 
                    onChange={e => setFormData({...formData, status: parseInt(e.target.value)})}
                    className="status-select"
                  >
                    <option value={1}>At Work</option>
                    <option value={2}>On Vacation</option>
                    <option value={3}>Sick Leave</option>
                  </select>
                </div>
              )}

              {user.role === 'Doctor' && (
                <div className="info-item">
                  <label>Career start year</label>
                  <input 
                    type="number"
                    disabled={!isEditing} 
                    value={formData.careerStartYear || 0} 
                    onChange={e => setFormData({...formData, careerStartYear: e.target.value})}
                  />
                </div>
              )}

              {(user.role === 'Doctor' || user.role === 'Receptionist') && (
                <div className="info-item">
                  <label>Office ID</label>
                  <input 
                    disabled={!isEditing} 
                    value={formData.officeId || ""} 
                    onChange={e => setFormData({...formData, officeId: e.target.value})}
                  />
                </div>
              )}

              {(user.role === 'Doctor' || user.role === 'Receptionist') && (
                <div className="info-item">
                  <label>Specialization</label>
                  <input 
                    disabled={!isEditing} 
                    value={formData.specializationName || ""} 
                    onChange={e => setFormData({...formData, officeId: e.target.value})}
                  />
                </div>
              )}
            </div>

            <div className="profile-actions">
              {!isEditing ? (
                <button className="action-btn edit" onClick={() => setIsEditing(true)}>Edit</button>
              ) : (
                <div className="edit-buttons">
                  <button className="action-btn save" onClick={handleSave}>Save</button>
                  <button className="action-btn cancel" onClick={() => {setIsEditing(false); setFormData(profile);}}>Cancel</button>
                </div>
              )}
            </div>
          </div>

        </div>
      </div>
    </div>
  );
}