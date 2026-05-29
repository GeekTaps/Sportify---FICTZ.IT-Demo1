import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import CrearModificarTurnoForm from "../components/FrontTurnos/CrearModificarTurnoForm";

function CrearModificarTurnoPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isModifying = !!id;

  const [deportes, setDeportes] = useState([]);
  const [fecha, setFecha] = useState("");
  const [cupo, setCupo] = useState("");
  const [idDeporte, setIdDeporte] = useState("");
  const [nombreTurno, setNombreTurno] = useState("");
  const [nommbreProfesor, setNommbreProfesor] = useState("");
  const [horaInicio, setHoraInicio] = useState("");
  const [horaFin, setHoraFin] = useState("");
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
          setFecha(data.fecha ? data.fecha.split("T")[0] : "");
          setCupo(data.cupo);
          setIdDeporte(data.idDeporte);
          setNombreTurno(data.nombreTurno || "");
          setNommbreProfesor(data.nommbreProfesor || "");
          setHoraInicio(data.horaInicio);
          setHoraFin(data.horaFin);
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

    if (!fecha || !cupo || !idDeporte || !horaInicio || !horaFin) {
      setError("Debes completar todos los campos.");
      return;
    }

    if (!nommbreProfesor.trim()) {
      setError("El nombre del profesor no puede estar vacío.");
      return;
    }

    if (horaInicio >= horaFin) {
      setError("La hora de inicio debe ser anterior a la hora de fin.");
      return;
    }

    // Validar que la duración sea exactamente 1 hora
    const [hInicio, mInicio] = horaInicio.split(":").map(Number);
    const [hFin, mFin] = horaFin.split(":").map(Number);
    const minutosDuracion = (hFin * 60 + mFin) - (hInicio * 60 + mInicio);
    if (minutosDuracion !== 60) {
      setError("La duración del turno debe ser exactamente 1 hora.");
      return;
    }

    setLoading(true);

    const turnoData = {
      id: isModifying ? id : "00000000-0000-0000-0000-000000000000",
      fecha: `${fecha}T${horaInicio}:00`,
      cupo: parseInt(cupo),
      idDeporte: idDeporte,
      nombreTurno: "Autogenerado", // El backend se encarga de cambiarlo
      nommbreProfesor: nommbreProfesor.trim(),
      horaInicio: horaInicio,
      horaFin: horaFin,
    };

    try {
      const url = isModifying
        ? `http://localhost:5266/api/turnos/${id}`
        : "http://localhost:5266/api/turnos";
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
      <div className="info-panel">
        <strong>Información importante:</strong>
        <ul>
          <li>La duración del turno debe ser <strong>exactamente 1 hora</strong></li>
          <li>La fecha no puede ser anterior a la fecha y hora actual</li>
          <li>El cupo debe ser mayor a 0</li>
          <li>Todos los campos son obligatorios</li>
        </ul>
      </div>

      <CrearModificarTurnoForm
        fecha={fecha}
        setFecha={setFecha}
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
        horaFin={horaFin}
        setHoraFin={setHoraFin}
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
        className="btn btn-outline"
        style={{ marginTop: "1rem" }}
      >
        ← Volver a Turnos
      </button>
    </div>
  );
}

export default CrearModificarTurnoPage;
