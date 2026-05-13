import {useState, useEffect} from "react";
import { fetchWithAuth } from "../api/auth";
import { useNavigate } from "react-router-dom";

export default function UsersListPage() {
  const [users, setUsers] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    async function loadUsers() {
      try {
        const data = await fetchWithAuth("https://localhost:7260/api/profiles/receptionist/all-users");
        setUsers(data);
      } catch (e) {
        console.error("Failed to load users", e);
      }
    }
    loadUsers();
  }, []);

  return (
    <div className="users-list-container">
      <h2>User Registry</h2>
      <table className="users-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Role</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {users.map(u => (
            <tr key={u.accountId}>
              <td>{u.lastName} {u.firstName}</td>
              <td>{u.role}</td>
              <td>
                <button onClick={() => navigate(`/profile/${u.role.toLowerCase()}/${u.accountId}`)}>
                  Edit Profile
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}