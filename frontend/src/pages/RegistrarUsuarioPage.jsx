import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import RegisterForm from "../components/FrontUsuarios/RegistrarUsuarioForm";
import { apiClient } from "../api/api-client";
import logoIcono from "../assets/logo_dibujito_sportify.png";
import logoLetras from "../assets/logo_letras_sportify-no-bg.png";

function RegistrarUsuarioPage() {
  const navigate = useNavigate();

  const [nombreCompleto, setNombreCompleto] = useState("");

  const [dni, setDni] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);
const [fechaNacimiento, setFechaNacimiento] = useState("");

const calcularEdad = (fecha) => {
  const hoy = new Date();
  const nacimiento = new Date(fecha);

  let edad = hoy.getFullYear() - nacimiento.getFullYear();

  if (
    hoy.getMonth() < nacimiento.getMonth() ||
    (hoy.getMonth() === nacimiento.getMonth() &&
      hoy.getDate() < nacimiento.getDate())
  ) {
    edad--;
  }

  return edad.toString();
};
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
      await apiClient.post("/usuarios/register", {
  nombreCompleto,
  fechaNacimiento,
  dni,
  email,
  password,
});

      setSuccess("Usuario registrado correctamente");
      setTimeout(() => navigate("/"), 1000);
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message ?? "Error al registrar usuario.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-split-page">
      {/* Panel izquierdo */}
      <div className="auth-split-left">
        <img src={logoIcono} alt="" className="auth-split-logo-icon" />
        <img src={logoLetras} alt="Sportify" className="auth-split-letters" />
        <h2>Únete a Sportify</h2>
        <p>
          Creá tu cuenta, explorá deportes y empezá a reservar tus clases favoritas
          en pocos pasos.
        </p>
      </div>

      {/* Panel derecho — formulario */}
      <div className="auth-split-right">
        <div className="auth-split-form">
          <h2>Crear Cuenta</h2>
          <p className="auth-split-form-subtitle">Completá tus datos para registrarte</p>

          <RegisterForm
            nombreCompleto={nombreCompleto}
            setNombreCompleto={setNombreCompleto}
           fechaNacimiento={fechaNacimiento}
            setFechaNacimiento={setFechaNacimiento}
            dni={dni}
            setDni={setDni}
            email={email}
            setEmail={setEmail}
            password={password}
            setPassword={setPassword}            confirmPassword={confirmPassword}
            setConfirmPassword={setConfirmPassword}            onSubmit={handleRegister}
            loading={loading}
            error={error}
            success={success}
          />

          <div className="auth-split-form-footer">
            ¿Ya tenés cuenta?{" "}
            <Link to="/login">Iniciá sesión</Link>
          </div>
        </div>
      </div>
    </div>
  );
}

export default RegistrarUsuarioPage;
