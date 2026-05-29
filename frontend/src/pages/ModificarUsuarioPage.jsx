import { useState, useContext } from "react";
import { useNavigate, useParams } from "react-router-dom";
import ModificarUsuarioForm from "../components/FrontUsuarios/ModificarUsuarioForm";
import { apiClient } from "../api/api-client";
import { AuthContext } from "../context/AuthContext";

function ModificarUsuarioPage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const { logout } = useContext(AuthContext);

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
      setError("Debes modificar al menos un campo..");
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
      setTimeout(() => navigate("/"), 1000);
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message ?? "Error al modificar usuario.");
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    if (!window.confirm("¿Seguro que querés borrar tu cuenta? Esta acción no se puede deshacer.")) {
      return;
    }

    try {
      await apiClient.post(`/usuarios/${id}/baja`);
      logout();
      setSuccess("Cuenta eliminada correctamente.");
      setTimeout(() => navigate("/"), 1000);
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message ?? "Error al eliminar la cuenta.");
    }
  };

  return (
    <div>
      <div className="page-header" style={{ textAlign: "left" }}>
        <h1>Modificar Datos</h1>
        <p>Actualizá tu información personal. Dejá en blanco lo que no quieras cambiar.</p>
      </div>

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

      <div
        style={{
          maxWidth: "440px",
          marginTop: "2rem",
          paddingTop: "1.5rem",
          borderTop: "1px solid var(--border)",
        }}
      >
        <button className="btn btn-danger" onClick={handleDelete}>
          Borrar mi cuenta
        </button>
      </div>
    </div>
  );
}

export default ModificarUsuarioPage;
