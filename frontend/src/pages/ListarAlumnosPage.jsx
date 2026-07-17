import { useState, useEffect } from "react";
import { apiClient } from "../api/api-client";

function AlumnosPage() {
  const [alumnos, setAlumnos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [seleccionado, setSeleccionado] = useState(null);
  const [soloSuspendidos, setSoloSuspendidos] = useState(false);
  const [busqueda, setBusqueda] = useState("");
  const [mensajeOperacion, setMensajeOperacion] = useState("");


  const fetchAlumnos = async (filtro = "todos") => {
    setLoading(true);
    setSeleccionado(null);

    try {
      let url = "/usuarios";

      if (filtro === "suspendidos") {
        url = "/usuarios/suspendidos";
      }

      const response = await apiClient.get(url);
      setAlumnos(response.data);
    } catch {
      setError("Error al cargar los alumnos.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAlumnos();
  }, []);
  
const toggleFiltro = () => {
    const nuevo = !soloSuspendidos;

    setSoloSuspendidos(nuevo);

    fetchAlumnos(nuevo ? "suspendidos" : "todos");
};


  if (loading) return <p>Cargando...</p>;
  if (error) return <div className="alert alert-error">{error}</div>;
  const alumnosFiltrados = alumnos.filter(alumno =>
    alumno.nombreCompleto.toLowerCase().includes(busqueda.toLowerCase())
  );
  return (
    <div className="page-container" style={{ display: "flex", gap: "2rem" }}>
      <div style={{ flex: 1 }}>
        <input
          type="text"
          placeholder="Buscar por nombre..."
          value={busqueda}
          onChange={(e) => setBusqueda(e.target.value)}
          className="form-control"
          style={{ marginBottom: "1rem", width: "100%" }}
        />
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
          <h2>Alumnos</h2>
          <button
            className={soloSuspendidos ? "btn btn-primary" : "btn btn-outline"}
            onClick={toggleFiltro}
          >
            {soloSuspendidos ? "Ver todos" : "Ver suspendidos"}
          </button>


        </div>

        <table className="tabla">
          <thead>
            <tr>
              <th>Nombre</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {alumnosFiltrados.map((alumno, index) => (
              <tr key={index}>
                <td>{alumno.nombreCompleto}</td>
                <td>
                  <button
                    className="btn btn-secondary"
                    onClick={() => setSeleccionado(alumno)}
                  >
                    Ver datos
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {seleccionado && (
        <div style={{
          width: "300px",
          border: "1px solid var(--c-cian-suave)",
          borderRadius: "8px",
          padding: "1.5rem",
          alignSelf: "flex-start"
        }}>
          {console.log(seleccionado)}
          <h3>Datos del alumno</h3>
          <p><strong>Nombre:</strong> {seleccionado.nombreCompleto}</p>
          <p><strong>Email:</strong> {seleccionado.email}</p>
          <p><strong>DNI:</strong> {seleccionado.dni}</p>
          <p><strong>Fecha de nacimiento:</strong> {new Date(seleccionado.fechaNacimiento).toLocaleDateString("es-AR")}</p>
          <p style={{
            color: seleccionado.suspendidoPermanente || seleccionado.suspendido ? "var(--c-rojo-hover)" : "var(--c-cian)",
            fontWeight: "bold"
          }}>
            Estado: {
              seleccionado.suspendidoPermanente ? "Suspendido permanentemente" :
                (seleccionado.suspendido ? "Suspendido temporalmente" : "Activo")
            }
          </p>

          <button
            className="btn btn-outline"
            style={{ marginTop: "1rem", width: "100%" }}
            onClick={() => setSeleccionado(null)}
          >
            Cerrar
          </button>

          {(!seleccionado.suspendido && !seleccionado.suspendidoPermanente) ? (
            <div style={{ display: "flex", flexDirection: "column", gap: "0.5rem", marginTop: "0.5rem" }}>
              <button
                className="btn btn-danger"
                style={{ width: "100%" }}
                onClick={async () => {
                  if (window.confirm("¿Estás seguro de que deseas suspender permanentemente a este usuario?")) {
                    const response = await apiClient.post(`/usuarios/suspender/${seleccionado.email}?cancelarReservas=false`);
                    setMensajeOperacion(response.data.message);
                    setTimeout(() => setMensajeOperacion(""), 3000);
                    setSeleccionado(null);
                    fetchAlumnos(soloSuspendidos ? "suspendidos" : "todos");
                  }
                }}
              >
                Suspender (permanente)
              </button>
              {/* 
              <button
                className="btn btn-danger"
                style={{ width: "100%" }}
                onClick={async () => {
                  if (window.confirm("¿Estás seguro de que deseas suspender permanentemente a este usuario y CANCELAR TODAS SUS RESERVAS?")) {
                    const response = await apiClient.post(`/usuarios/suspender/${seleccionado.email}?cancelarReservas=true`);
                    setMensajeOperacion(response.data.message);
                    setTimeout(() => setMensajeOperacion(""), 3000);
                    setSeleccionado(null);
                    fetchAlumnos(soloSuspendidos ? "suspendidos" : "todos");
                  }
                }}
              >
                Suspender y Cancelar Reservas (permanente)
              </button>
              */}
            </div>
          ) : (
            <button
              className="btn btn-primary"
              style={{ marginTop: "0.5rem", width: "100%" }}
              onClick={async () => {
                const response = await apiClient.post(`/usuarios/reactivar/${seleccionado.email}`);
                setMensajeOperacion(response.data.message);
                setTimeout(() => setMensajeOperacion(""), 3000);
                setSeleccionado(null);
                fetchAlumnos(soloSuspendidos ? "suspendidos" : "todos");
              }}
            >
              Reactivar alumno
            </button>
          )}
        </div>
      )}

      {mensajeOperacion && (
        <div
          className="alert alert-success"
          style={{
            position: 'fixed',
            top: '20px',
            right: '20px',
            zIndex: 9999,
            boxShadow: '0 4px 12px rgba(0,0,0,0.15)',
            animation: 'fadeIn 0.3s ease-in-out',
            minWidth: '250px',
            textAlign: 'center'
          }}
        >
          {mensajeOperacion}
        </div>
      )}
    </div>
  );
}

export default AlumnosPage;