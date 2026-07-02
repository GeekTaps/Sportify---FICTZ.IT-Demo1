import { useState } from "react";
import { useNavigate } from "react-router-dom";
import RegistrarEmpleadoForm from "../components/FrontUsuarios/RegistrarEmpleadoForm";
import { apiClient } from "../api/api-client";
import logoIcono from "../assets/logo_dibujito_sportify.png";
import logoLetras from "../assets/logo_letras_sportify-no-bg.png";

function RegistrarEmpleadoPage() {
  const navigate = useNavigate();

  const [nombreCompleto, setNombreCompleto] = useState("");
  const [dni, setDni] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [fechaNacimiento, setFechaNacimiento] = useState("");

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  const handleRegister = async (event) => {
    event.preventDefault();

    setError("");
    setSuccess("");

    const requiredFields = [
      nombreCompleto,
      fechaNacimiento,
      dni,
      email,
      password,
      confirmPassword,
    ];

    const hasEmptyField = requiredFields.some(
      (field) => !field?.toString().trim()
    );

    if (hasEmptyField) {
      setError("Debes completar todos los campos.");
      return;
    }

    if (password !== confirmPassword) {
      setError("Las contraseñas no coinciden.");
      return;
    }

    setLoading(true);

    try {
      await apiClient.post("/empleados", {
        nombreCompleto,
        fechaNacimiento,
        dni,
        email,
        password,
      });

      setSuccess("Empleado registrado correctamente.");

      setTimeout(() => {
        navigate("/");
      }, 1000);

    } catch (err) {
      console.error(err);
      setError(
        err.response?.data?.message ??
        "Error al registrar empleado."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-split-page">

      <div className="auth-split-left">
        <img src={logoIcono} alt="" className="auth-split-logo-icon" />
        <img src={logoLetras} alt="Sportify" className="auth-split-letters" />

        <h2>Registrar Empleado</h2>

        <p>
          Desde esta sección los administradores pueden crear cuentas de
          empleados para gestionar la plataforma.
        </p>
      </div>

      <div className="auth-split-right">
        <div className="auth-split-form">

          <h2>Nuevo Empleado</h2>

          <p className="auth-split-form-subtitle">
            Completá los datos del empleado.
          </p>

<RegistrarEmpleadoForm
    nombreCompleto={nombreCompleto}
    setNombreCompleto={setNombreCompleto}
    fechaNacimiento={fechaNacimiento}
    setFechaNacimiento={setFechaNacimiento}
    dni={dni}
    setDni={setDni}
    email={email}
    setEmail={setEmail}
    password={password}
    setPassword={setPassword}
    confirmPassword={confirmPassword}
    setConfirmPassword={setConfirmPassword}
    onSubmit={handleRegister}
    loading={loading}
    error={error}
    success={success}
/>

        </div>
      </div>

    </div>
  );
}

export default RegistrarEmpleadoPage;