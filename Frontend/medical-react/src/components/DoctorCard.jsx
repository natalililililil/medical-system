export default function DoctorCard({ doctor }) {
  return (
    <div className="doctor-card">
      <img 
        src={doctor.photoUrl || 'https://via.placeholder.com/150'} 
        alt={doctor.fullName} 
        className="doctor-photo"
      />
      <div className="doctor-info">
        <h3>{doctor.fullName}</h3>
        <p><strong>Specialization:</strong> {doctor.specializationName}</p>
        <p><strong>Experience:</strong> {doctor.experience} years</p>
        <p><strong>Office ID:</strong> {doctor.officeId}</p>
        <span className="doctor-status"> At Work</span>
      </div>
    </div>
  );
}