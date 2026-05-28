import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import ModificarUsuarioForm from "../components/FrontUsuarios/ModificarUsuarioForm";
import { apiClient } from "../api/api-client";

function ModificarUsuarioPage() {
    
  const navigate = useNavigate();
  const { id } = useParams();

  const [nombreCompleto, setNombreCompleto] = useState("");
  const [edad, setEdad] = useState("");
  const [dni, setDni] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  const handleUpdate = async (event) => {
    event.preventDefault();

    setError("");
    setSuccess("");

    if (
      !nombreCompleto.trim() &&
      !email.trim() &&
      !dni.trim() &&
      !edad.trim() &&
      !password.trim()
    ) {
      setError("Debes modificar al menos un campo.");
      return;
    }

    setLoading(true);

    try {
      await apiClient.patch(`/usuarios/${id}`, {
        nombreCompleto,
        edad,
        dni,
        email,
        password,
      });

      setSuccess("Usuario modificado correctamente.");

      setTimeout(() => {
        navigate("/");
      }, 1000);

    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message ?? "Error al modificar usuario.");
    } finally {
      setLoading(false);
    }
  };

  
  const handleDelete = async () => {
    const confirmDelete = window.confirm("¿Seguro que querés borrar tu cuenta?");
    if (!confirmDelete) return;

    try {
      await apiClient.post(`/usuarios/${id}/baja`);

      setSuccess("Cuenta eliminada correctamente");

      setTimeout(() => {
        navigate("/");
      }, 1000);

    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message ?? "Error al eliminar la cuenta");
    }
  };

  return (
    <div>
      <h1>Modificar Datos</h1>

      <ModificarUsuarioForm
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
        onSubmit={handleUpdate}
        loading={loading}
        error={error}
        success={success}
      />

      <button
        style={{ marginTop: "20px", background: "red", color: "white" }}
        onClick={handleDelete}
      >
        Borrar mi cuenta
      </button>
    </div>
  );
}
export default ModificarUsuarioPage;