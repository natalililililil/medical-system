export default function DoctorCard({ doctor, specializations }) {
  const currentSpec = specializations.find(s => s.id === doctor.specializationId);
  const specName = currentSpec ? currentSpec.name : "Specification not selected";

  return (
    <div className="doctor-card">
      <img 
        src={doctor.photoUrl || 'https://via.placeholder.com/150'} 
        alt={doctor.fullName} 
        className="doctor-photo"
      />
      <div className="doctor-info">
        <h3>{doctor.fullName}</h3>
        <p><strong>Specialization:</strong> {specName}</p>
        <p><strong>Experience:</strong> {new Date().getFullYear() - doctor.careerStartYear + 1} years</p>
        <p><strong>Office ID:</strong> {doctor.officeId}</p>
        <span className="doctor-status"> At Work</span>
      </div>
    </div>
  );
}