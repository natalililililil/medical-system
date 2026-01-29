import { useState } from "react";
import { login } from "../api/auth";
import "../App.css";

export default function LoginModal({ onClose }) {
  const [values, setValues] = useState({ email: "", password: "" });
  const [touched, setTouched] = useState({});
  const [serverError, setServerError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  function handleChange(e) {
    setValues(v => ({ ...v, [e.target.name]: e.target.value }));
  }

  function handleBlur(e) {
    setTouched(t => ({ ...t, [e.target.name]: true }));
  }

  function validate(values) {
    const errors = {};
    if (!values.email) errors.email = "Please, enter the email";
    else if (!/\S+@\S+\.\S+/.test(values.email)) errors.email = "You've entered an invalid email";

    if (!values.password) errors.password = "Please, enter the password";
    return errors;
  }

  const errors = validate(values);
  const isValid = Object.keys(errors).length === 0;

  async function handleSubmit(e) {
    e.preventDefault();
    setServerError("");
    setSuccessMessage("");

    try {
      await login(values);
      setSuccessMessage("Вы успешно вошли!");
      setTimeout(() => onClose(), 1500);
    } catch (err) {
      setServerError(err.message);
    }
  }

  return (
    <div className="modal">
      <button className="close-button" onClick={onClose}>×</button>
      <form onSubmit={handleSubmit}>
        <h2>Вход</h2>

        <input
          name="email"
          placeholder="Email"
          value={values.email}
          onChange={handleChange}
          onBlur={handleBlur}
          className={touched.email && errors.email ? "input-error" : ""}
        />
        {touched.email && errors.email && <p className="error-message">{errors.email}</p>}

        <input
          type="password"
          name="password"
          placeholder="Пароль"
          value={values.password}
          onChange={handleChange}
          onBlur={handleBlur}
          className={touched.password && errors.password ? "input-error" : ""}
        />
        {touched.password && errors.password && <p className="error-message">{errors.password}</p>}

        {serverError && <p className="error-message">{serverError}</p>}
        {successMessage && <p style={{ color: "green", marginBottom: "10px" }}>{successMessage}</p>}

        <button type="submit" disabled={!isValid}>Войти</button>
      </form>
    </div>
  );
}