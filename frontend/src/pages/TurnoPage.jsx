import BotonModificarTurno from "../components/FrontTurnos/BotonModificarTurno";
import BotonMostrarListadoTurnos from "../components/FrontTurnos/BotonMostrarListadoTurnos";
import BotonCrearTurno from "../components/FrontTurnos/BotonCrearTurno";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

function TurnoPage() {
  const [turnos, setTurnos] = useState([]);
  const navigate = useNavigate();

  const cargarTurnos = async () => {
    try {
      const response = await fetch("http://localhost:5266/api/turnos");
      if (!response.ok) {
        throw new Error(`Error HTTP ${response.status}`);
      }
      const data = await response.json();
      setTurnos(data);
    } catch (error) {
      console.error("Error al cargar turnos:", error);
      alert("Error al cargar los turnos. Asegúrate de que el backend esté ejecutándose.");
    }
  };

  const modificarTurno = (id) => {
    navigate(`/turnos/modificar/${id}`);
  };

  const formatearHora = (horaString) => {
    if (!horaString) return "N/A";
    // Si es un string de tiempo HH:MM, devolverlo tal cual
    if (horaString.length === 5) return horaString;
    // Si es un objeto con horas y minutos
    if (typeof horaString === "object" && horaString.hours !== undefined) {
      return `${String(horaString.hours).padStart(2, "0")}:${String(horaString.minutes).padStart(2, "0")}`;
    }
    return horaString;
  };

  const formatearFecha = (fechaString) => {
    if (!fechaString) return "N/A";
    const fecha = new Date(fechaString);
    return fecha.toLocaleDateString("es-ES");
  };

  return (
    <div>
      <h1>Gestión de Turnos</h1>
      <p>Administra los turnos disponibles en el sistema.</p>

      <div style={{ marginBottom: "20px" }}>
        <BotonCrearTurno />
        <BotonMostrarListadoTurnos onClick={cargarTurnos} />
      </div>

      {turnos.length === 0 ? (
        <p>No hay turnos cargados. Haz clic en "Mostrar Listado de Turnos" para cargar los turnos disponibles.</p>
      ) : (
        <ul style={{ listStyle: "none", padding: 0 }}>
          {turnos.map((turno) => (
            <li
              key={turno.id}
              style={{
                marginBottom: "15px",
                padding: "15px",
                border: "1px solid #ddd",
                borderRadius: "4px",
                backgroundColor: "#f9f9f9",
              }}
            >
              <div>
                <strong>Nombre del Turno:</strong> {turno.nombreTurno || "N/A"}
              </div>
              <div>
                <strong>Fecha:</strong> {formatearFecha(turno.fecha)}
              </div>
              <div>
                <strong>Hora Inicio:</strong> {formatearHora(turno.horaInicio)}
              </div>
              <div>
                <strong>Hora Fin:</strong> {formatearHora(turno.horaFin)}
              </div>
              <div>
                <strong>Cupo:</strong> {turno.cupo} personas
              </div>
              <div>
                <strong>Profesor:</strong> {turno.nommbreProfesor || "N/A"}
              </div>
              <BotonModificarTurno onClick={() => modificarTurno(turno.id)} />
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default TurnoPage;
