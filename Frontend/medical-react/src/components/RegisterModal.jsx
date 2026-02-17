import { useState } from "react";
import { register } from "../api/auth";
import "../App.css";

export default function RegisterModal({ onSuccess, onClose }) {
  const [values, setValues] = useState({
    email: "",
    password: "",
    confirmPassword: ""
  });

  const [touched, setTouched] = useState({});
  const [serverError, setServerError] = useState("");

  function handleChange(e) {
    setValues(v => ({ ...v, [e.target.name]: e.target.value }));
  }

  function handleBlur(e) {
    setTouched(t => ({ ...t, [e.target.name]: true }));
  }

  function validate(values) {
    const errors = {};

    if (!values.email) {
      errors.email = "Please, enter the email";
    } else if (!/\S+@\S+\.\S+/.test(values.email)) {
      errors.email = "You've entered an invalid email";
    }

    if (!values.password) {
      errors.password = "Please, enter the password";
    } else if (values.password.length < 6) {
      errors.password = "Password must be at least 6 symbols";
    }

    if (!values.confirmPassword) {
      errors.confirmPassword = "Please, reenter the password";
    } else if (values.password !== values.confirmPassword) {
      errors.confirmPassword = "The passwords you’ve entered don’t coincide";
    }

    return errors;
  }

  const errors = validate(values);
  const isValid = Object.keys(errors).length === 0;

  async function handleSubmit(e) {
    e.preventDefault();
    try {
      await register(values);
      onSuccess();
    } catch (err) {
      setServerError(err.message);
    }
  }

  return (
    <div className="modal">
      <button className="close-button" onClick={onClose}>×</button>

      <form onSubmit={handleSubmit}>
        <h2>Sign Up</h2>

        <input
          name="email"
          placeholder="Email"
          value={values.email}
          onChange={handleChange}
          onBlur={handleBlur}
          className={touched.email && errors.email ? "input-error" : ""}
        />
        {touched.email && errors.email && (
          <p className="error-message">{errors.email}</p>
        )}

        <input
          type="password"
          name="password"
          placeholder="Password"
          value={values.password}
          onChange={handleChange}
          onBlur={handleBlur}
          className={touched.password && errors.password ? "input-error" : ""}
        />
        {touched.password && errors.password && (
          <p className="error-message">{errors.password}</p>
        )}

        <input
          type="password"
          name="confirmPassword"
          placeholder="Confirm password"
          value={values.confirmPassword}
          onChange={handleChange}
          onBlur={handleBlur}
          className={
            touched.confirmPassword && errors.confirmPassword
              ? "input-error"
              : ""
          }
        />
        {touched.confirmPassword && errors.confirmPassword && (
          <p className="error-message">{errors.confirmPassword}</p>
        )}

        {serverError && (
          <p className="error-message">{serverError}</p>
        )}

        <button type="submit" disabled={!isValid}>
          Sign up
        </button>
      </form>
    </div>
  );
}