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

  const [originalNombreCompleto, setOriginalNombreCompleto] = useState("");
  const [originalFechaNacimiento, setOriginalFechaNacimiento] = useState("");
  const [originalDni, setOriginalDni] = useState("");
  const [originalEmail, setOriginalEmail] = useState("");

  const [creditos, setCreditos] = useState([]);
  const [abonos, setAbonos] = useState([]);
  const [suspendido, setSuspendido] = useState(false);
  const [suspendidoPermanente, setSuspendidoPermanente] = useState(false);

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);
  const [userLoading, setUserLoading] = useState(true);
  const { id } = useParams();
  const userId = id || user?.id;

  useEffect(() => {
    if (!userId) {
      setUserLoading(false);
      return;
    }

    const cargarUsuario = async () => {
      setUserLoading(true);
      setError("");

      try {
        const response = await apiClient.get(`/usuarios/${userId}`);

        const nombre = response.data.nombreCompleto ?? "";
        const mail = response.data.email ?? "";
        const documento = response.data.dni ?? "";
        const fecha = response.data.fechaNacimiento?.split("T")[0] ?? "";

        setNombreCompleto(nombre);
        setEmail(mail);
        setDni(documento);
        setFechaNacimiento(fecha);

        setOriginalNombreCompleto(nombre);
        setOriginalEmail(mail);
        setOriginalDni(documento);
        setOriginalFechaNacimiento(fecha);
        
        setCreditos(response.data.creditos ?? []);
        setAbonos(response.data.abonos ?? []);
        setSuspendido(response.data.suspendido ?? false);
        setSuspendidoPermanente(response.data.suspendidoPermanente ?? false);
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

    const noHayCambios =
      nombreCompleto === originalNombreCompleto &&
      email === originalEmail &&
      dni === originalDni &&
      fechaNacimiento === originalFechaNacimiento &&
      !passwordNueva.trim();

    if (noHayCambios) {
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

      <div style={{ marginBottom: "2rem", maxWidth: "440px" }}>
        <div style={{ marginBottom: "1.5rem", padding: "1rem", borderRadius: "8px", backgroundColor: "var(--bg-secondary)", border: "1px solid var(--border)" }}>
          <h3 style={{ marginTop: 0, marginBottom: "0.5rem" }}>Estado de la Cuenta</h3>
          <p style={{ margin: 0, fontWeight: "bold", color: suspendidoPermanente ? "var(--c-rojo-coral)" : (suspendido ? "var(--c-naranja)" : "var(--c-cian-brillante)") }}>
            {suspendidoPermanente 
              ? "Suspendido Indefinidamente" 
              : suspendido 
                ? "Suspendido Temporalmente (hasta el día 11)" 
                : "Activo"}
          </p>
        </div>

        <h3>Mis Créditos</h3>
        {creditos.length > 0 ? (
          <ul style={{ paddingLeft: "20px", marginBottom: "1.5rem" }}>
            {creditos.map((c, idx) => (
              <li key={idx} style={{ marginBottom: "0.5rem" }}>
                <strong>{c.deporte}:</strong> {c.cantidad} crédito(s)
              </li>
            ))}
          </ul>
        ) : (
          <p style={{ marginBottom: "1.5rem" }}>No tenés créditos disponibles.</p>
        )}

        <h3>Mis Abonos</h3>
        {abonos.length > 0 ? (
          <ul style={{ paddingLeft: "20px", marginBottom: "1.5rem" }}>
            {abonos.map((a, idx) => {
              const diasTraduccion = {
                "Monday": "Lunes",
                "Tuesday": "Martes",
                "Wednesday": "Miércoles",
                "Thursday": "Jueves",
                "Friday": "Viernes",
                "Saturday": "Sábado",
                "Sunday": "Domingo"
              };
              const diaEspanol = diasTraduccion[a.dia] || a.dia;

              return (
                <li key={idx} style={{ marginBottom: "0.5rem" }}>
                  Abonado en {a.deporte} - {diaEspanol} - {a.hora}
                </li>
              );
            })}
          </ul>
        ) : (
          <p style={{ marginBottom: "1.5rem" }}>No tenés abonos activos.</p>
        )}
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
