import { useState, useEffect } from "react";
import { apiClient } from "../api/api-client";

function AlumnosPage() {
  const [alumnos, setAlumnos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [seleccionado, setSeleccionado] = useState(null);
  const [soloSuspendidos, setSoloSuspendidos] = useState(false);
  const [busqueda, setBusqueda] = useState("");

  const fetchAlumnos = async (suspendidos = false) => {
    setLoading(true);
    setSeleccionado(null);
    try {
      const url = suspendidos ? "/usuarios/suspendidos" : "/usuarios";
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
    fetchAlumnos(nuevo);
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
          <button
            className="btn btn-outline"
            style={{ marginTop: "1rem", width: "100%" }}
            onClick={() => setSeleccionado(null)}
          >
            Cerrar
          </button>
          {seleccionado.suspendido && (
  <button
    className="btn btn-primary"
    style={{ marginTop: "0.5rem", width: "100%" }}
    onClick={async () => {
      await apiClient.post(`/usuarios/reactivar/${seleccionado.email}`);
      setSeleccionado(null);
      fetchAlumnos(soloSuspendidos);
    }}
  >
    Reactivar alumno
  </button>
)}      
        </div>
      )}
    </div>
  );
}

export default AlumnosPage;