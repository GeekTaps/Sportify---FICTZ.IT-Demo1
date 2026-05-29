import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import RegisterForm from "../components/FrontUsuarios/RegistrarUsuarioForm";
import { apiClient } from "../api/api-client";
import logoIcono from "../assets/logo_dibujito_sportify.png";
import logoLetras from "../assets/logo_letras_sportify-no-bg.png";

function RegistrarUsuarioPage() {
  const navigate = useNavigate();

  const [nombreCompleto, setNombreCompleto] = useState("");
  const [edad, setEdad] = useState("");
  const [dni, setDni] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  const handleRegister = async (event) => {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (!nombreCompleto.trim() || !edad.trim() || !dni.trim() || !email.trim() || !password.trim()) {
      setError("Debes completar todos los campos.");
      return;
    }

    setLoading(true);

    try {
      await apiClient.post("/usuarios/register", {
        nombreCompleto,
        edad,
        dni,
        email,
        password,
      });

      setSuccess("Usuario registrado correctamente.");
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
            edad={edad}
            setEdad={setEdad}
            dni={dni}
            setDni={setDni}
            email={email}
            setEmail={setEmail}
            password={password}
            setPassword={setPassword}
            onSubmit={handleRegister}
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
