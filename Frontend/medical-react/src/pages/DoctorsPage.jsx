import { useEffect, useState } from "react";
import { getDoctors } from "../api/doctors";
import DoctorCard from "../components/DoctorCard";

export default function DoctorsPage() {
  const [doctors, setDoctors] = useState([]);
  const [specializations, setSpecializations] = useState([]);
  const [filters, setFilters] = useState({
    Name: "",
    SpecializationId: "",
    OfficeId: ""
  });

  useEffect(() => {
    async function loadSpecs() {
      try {
        const data = await fetchWithAuth("https://localhost:7260/api/profiles/doctor/specializations");
        setSpecializations(data);
      } catch (e) {
        console.error("Specs load error", e);
      }
    }
    loadSpecs();
  }, []);

  const loadDoctors = async () => {
    try {
      const data = await getDoctors(filters);
      
      const validAndWorking = data
        .map(d => ({
                ...d,
                fullName: d.fullName || `${d.lastName} ${d.firstName} ${d.middleName || ""}`.trim()
            }))
        .filter(d => {
          const hasName = d.fullName && d.fullName.trim() !== "";
          const isWorking = d.status === 1 || d.status === "AtWork";
          return hasName && isWorking;
        });
      
      setDoctors(validAndWorking);
    } catch (e) {
      console.error(e);
    }
  };

  useEffect(() => {
    loadDoctors();
  }, [filters]);

  const handleChange = (e) => {
    setFilters({ ...filters, [e.target.name]: e.target.value });
  };

  return (
   <div className="page-container">
      <h2>Our specialists</h2>

    <div className="filters-wrapper">
      <div className="filters-container">
        <select
          name="SpecializationId"
          value={filters.specializationId}
          onChange={handleChange}
          className="status-select"
        >
          <option value="">All Specializations</option>
          {specializations.map(spec => (
              <option key={spec.id} value={spec.id}>
                {spec.name}
              </option>
            ))}
        </select>

        <input
          name="Name"
          placeholder="Search by name..."
          value={filters.fullName}
          onChange={handleChange}
        />
        
        <input
          name="OfficeId"
          placeholder="Office Id"
          value={filters.officeId}
          onChange={handleChange}
        />
        <button className="register-button" onClick={() => alert("Map in development")}>View on map</button>
      </div>
    </div>
      

      <div className="doctors-grid">
        {doctors.length > 0 ? (
          doctors.map(d => <DoctorCard key={d.accountId} doctor={d} specializations={specializations}/>)
        ) : (
          <p>No doctors have been found or no one is working right now</p>
        )}
      </div>
    </div>
  );
}