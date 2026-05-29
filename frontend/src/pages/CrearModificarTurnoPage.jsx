import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import CrearModificarTurnoForm from "../components/FrontTurnos/CrearModificarTurnoForm";

function CrearModificarTurnoPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isModifying = !!id;

  const [deportes, setDeportes] = useState([]);
  const [fechaInicio, setFechaInicio] = useState("");
  const [cupo, setCupo] = useState("");
  const [idDeporte, setIdDeporte] = useState("");
  const [nombreTurno, setNombreTurno] = useState("");
  const [nommbreProfesor, setNommbreProfesor] = useState("");
  const [horaInicio, setHoraInicio] = useState("");
  const [precio, setPrecio] = useState("");
  const [listaEsperaHabilitada, setListaEsperaHabilitada] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const cargarDeportes = async () => {
      try {
        const response = await fetch("http://localhost:5266/api/deportes");
        if (!response.ok) throw new Error("Error al cargar deportes");
        const data = await response.json();
        setDeportes(data);
      } catch (error) {
        console.error("Error al cargar deportes:", error);
        setError("No se pudieron cargar los deportes.");
      }
    };

    const cargarTurno = async () => {
      if (isModifying) {
        try {
          const response = await fetch(`http://localhost:5266/api/turnos/${id}`);
          if (!response.ok) throw new Error("Error al cargar turno");
          const data = await response.json();
          if (data.fecha) {
            const dateObj = new Date(data.fecha);
            setFechaInicio(dateObj.toISOString().split("T")[0]);
          }
          setCupo(data.cupo);
          setIdDeporte(data.idDeporte);
          setNombreTurno(data.nombreTurno || "");
          setNommbreProfesor(data.nommbreProfesor || "");
          setHoraInicio(data.horaInicio);
          setPrecio(data.precio || "");
          setListaEsperaHabilitada(data.listaEsperaHabilitada || false);
        } catch (error) {
          console.error("Error al cargar turno:", error);
          setError("No se pudo cargar el turno.");
        }
      }
    };

    cargarDeportes();
    cargarTurno();
  }, [id, isModifying]);

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (!fechaInicio || !cupo || !precio || !idDeporte || !horaInicio || !nommbreProfesor) {
      setError("No puede haber campos en blanco");
      return;
    }

    const fechaSeleccionada = new Date(fechaInicio + "T00:00:00");
    const hoy = new Date();
    hoy.setHours(0, 0, 0, 0);

    if (fechaSeleccionada <= hoy) {
      setError("La fecha de inicio debe ser posterior a la fecha actual.");
      return;
    }

    if (parseInt(cupo) <= 0) {
      setError("El cupo debe ser mayor a 0");
      return;
    }

    if (parseFloat(precio) < 0) {
      setError("El precio no puede ser negativo");
      return;
    }

    setLoading(true);

    const turnoData = {
      idDeporte: idDeporte,
      fechaInicio: fechaInicio,
      horaInicio: horaInicio,
      cupo: parseInt(cupo),
      precio: parseFloat(precio),
      nombreProfesor: nommbreProfesor.trim(),
      listaEsperaHabilitada: listaEsperaHabilitada
    };

    try {
      const url = isModifying
        ? `http://localhost:5266/api/turnos/mensual/${id}`
        : "http://localhost:5266/api/turnos/mensual";
      const method = isModifying ? "PUT" : "POST";

      const response = await fetch(url, {
        method: method,
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(turnoData),
      });

      if (!response.ok) {
        const body = await response.json().catch(() => null);
        setError(body?.message ?? `No se pudo ${isModifying ? "modificar" : "crear"} el turno.`);
        return;
      }

      setSuccess(`Turno ${isModifying ? "modificado" : "creado"} con éxito.`);
      setTimeout(() => navigate("/turnos"), 1500);
    } catch (err) {
      console.error(err);
      setError(`Error al ${isModifying ? "modificar" : "crear"} el turno. Intenta nuevamente.`);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>{isModifying ? "Modificar Turno" : "Crear Nuevo Turno"}</h1>
      <p>
        {isModifying
          ? "Modifica los datos del turno."
          : "Ingresa los datos para crear un nuevo turno."}
      </p>
      <div style={{
        padding: "10px",
        backgroundColor: "#e3f2fd",
        borderLeft: "4px solid #2196F3",
        marginBottom: "20px",
        borderRadius: "4px"
      }}>
        <p style={{ margin: "5px 0" }}>
          <strong>Información importante:</strong>
        </p>
        <ul style={{ margin: "5px 0", paddingLeft: "20px" }}>
          <li>Todas las clases duran exactamente 1 hora</li>
          <li>Se crearán todos los turnos para la fecha seleccionada durante los próximos 30 días</li>
          <li>No puede haber más de un turno de una misma actividad en el mismo día y horario</li>
          <li>El cupo no puede ser 0</li>
        </ul>
      </div>

      <CrearModificarTurnoForm
        fechaInicio={fechaInicio}
        setFechaInicio={setFechaInicio}
        cupo={cupo}
        setCupo={setCupo}
        idDeporte={idDeporte}
        setIdDeporte={setIdDeporte}
        nombreTurno={nombreTurno}
        setNombreTurno={setNombreTurno}
        nommbreProfesor={nommbreProfesor}
        setNommbreProfesor={setNommbreProfesor}
        horaInicio={horaInicio}
        setHoraInicio={setHoraInicio}
        precio={precio}
        setPrecio={setPrecio}
        listaEsperaHabilitada={listaEsperaHabilitada}
        setListaEsperaHabilitada={setListaEsperaHabilitada}
        deportes={deportes}
        onSubmit={handleSubmit}
        loading={loading}
        error={error}
        success={success}
        isModifying={isModifying}
      />

      <button
        type="button"
        onClick={() => navigate("/turnos")}
        style={{
          marginTop: "15px",
          padding: "10px 20px",
          fontSize: "16px",
          backgroundColor: "#757575",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: "pointer",
        }}
      >
        Volver a Turnos
      </button>
    </div>
  );
}

export default CrearModificarTurnoPage;
