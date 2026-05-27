import { useState } from "react";
import { useNavigate } from "react-router-dom";
import RegisterForm from "../components/FrontUsuarios/RegistrarUsuarioForm";
import { apiClient } from "../api/api-client";

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

    if (
      !nombreCompleto.trim() ||
      !edad.trim() ||
      !dni.trim() ||
      !email.trim() ||
      !password.trim()
    ) {
      setError("Debes completar todos los campos.");
      return;
    }

    setLoading(true);

    try {
      await apiClient.post("/usuarios/register", {
        nombreCompleto,
         edad: Number(edad),
        dni,
        email,
        password,
      });

      setSuccess("Usuario registrado correctamente.");

      setTimeout(() => {
        navigate("/");
      }, 1000);

    } catch (err) {
      console.error(err);

      setError(
        err.response?.data?.message ??
        "Error al registrar usuario."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>Registro de Usuario</h1>

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
    </div>
  );
}

export default RegistrarUsuarioPage;