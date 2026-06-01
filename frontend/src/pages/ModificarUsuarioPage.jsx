import { useState, useContext, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import ModificarUsuarioForm from "../components/FrontUsuarios/ModificarUsuarioForm";
import { apiClient } from "../api/api-client";
import { AuthContext } from "../context/AuthContext";

function ModificarUsuarioPage() {
  const navigate = useNavigate();
 
  const { user, logout } = useContext(AuthContext);

  const [nombreCompleto, setNombreCompleto] = useState("");
  const [fechaNacimiento, setFechaNacimiento] = useState("");
  const [dni, setDni] = useState("");
  const [email, setEmail] = useState("");
  const [passwordActual, setPasswordActual] = useState("");
  const [passwordNueva, setPasswordNueva] = useState("");
  const [confirmarPassword, setConfirmarPassword] = useState("");

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);
  const [userLoading, setUserLoading] = useState(true);
  const { id } = useParams();
  const userId = id || user?.id;

  useEffect(() => {
    if (!userId) {
      setError("No se encontró el usuario.");
      setUserLoading(false);
      return;
    }

    const cargarUsuario = async () => {
      setUserLoading(true);
      setError("");

      try {
        const response = await apiClient.get(`/usuarios/${userId}`);

        setNombreCompleto(response.data.nombreCompleto ?? "");
        setEmail(response.data.email ?? "");
        setDni(response.data.dni ?? "");
        setFechaNacimiento(
          response.data.fechaNacimiento?.split("T")[0] ?? ""
        );
      } catch (err) {
        console.error("ERROR GET USER:", err.response?.data || err);
        setError("No se pudieron cargar los datos del usuario.");
      } finally {
        setUserLoading(false);
      }
    };

    cargarUsuario();
  }, [userId]);
  const handleUpdate = async (event) => {
  event.preventDefault();
  setError("");
  setSuccess("");

  if (
    !nombreCompleto.trim() &&
    !email.trim() &&
    !dni.trim() &&
    !fechaNacimiento.trim() &&
    !passwordNueva.trim()
  ) {
    setError("Debes modificar al menos un campo.");
    return;
  }

  if (passwordNueva !== confirmarPassword) {
    setError("Las contraseñas no coinciden");
    return;
  }

  setLoading(true);

  try {
    await apiClient.patch(`/usuarios/${userId}`, {
      nombreCompleto,
      fechaNacimiento,
      dni,
      email,
      passwordActual,
      passwordNueva,
    });

    setSuccess("Usuario modificado correctamente.");
  } catch (err) {
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
      await apiClient.post(`/usuarios/${userId}/baja`);
      logout();
      setSuccess("Cuenta eliminada correctamente.");
      setTimeout(() => navigate("/"), 1000);
    } catch (err) {
      console.error(err);
      setError(err.response?.data?.message ?? "Error al eliminar la cuenta.");
    }
  };

  if (userLoading) {
    return (
      <div>
        <div className="page-header" style={{ textAlign: "left" }}>
          <h1>Modificar Datos</h1>
          <p>Cargando los datos del usuario...</p>
        </div>
      </div>
    );
  }

  return (
    <div>
      <div className="page-header" style={{ textAlign: "left" }}>
        <h1>Modificar Datos</h1>
        <p>Actualizá tu información personal. Dejá en blanco lo que no quieras cambiar.</p>
      </div>

      <ModificarUsuarioForm
        nombreCompleto={nombreCompleto}
        setNombreCompleto={setNombreCompleto}
        fechaNacimiento={fechaNacimiento}
        setFechaNacimiento={setFechaNacimiento}
        dni={dni}
        setDni={setDni}
        email={email}
        setEmail={setEmail}
        passwordActual={passwordActual}
        setPasswordActual={setPasswordActual}
        passwordNueva={passwordNueva}
        setPasswordNueva={setPasswordNueva}
        confirmarPassword={confirmarPassword}
        setConfirmarPassword={setConfirmarPassword}
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
