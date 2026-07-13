import { useState } from "react";
import RegistrarDeporteForm from "../components/FrontDeportes/RegistrarDeporteForm";
import BotonMostrarListadoDeportes from "../components/FrontDeportes/BotonMostrarListadoDeportes";
import BotonModificarDeporte from "../components/FrontDeportes/BotonModificarDeporte";

function CrearDeportePage() {
  const [deportes, setDeportes] = useState([]);
  const [nombre, setNombre] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  const cargarDeportes = async () => {
    try {
      const response = await fetch("http://localhost:5266/api/deportes");
      if (!response.ok) {
        throw new Error(`Error HTTP ${response.status}`);
      }
      const data = await response.json();
      setDeportes(data);
    } catch (error) {
      console.error("Error al cargar deportes:", error);
    }
  };

  const modificarDeporte = (id) => {
    window.location.href = `/deportes/modificar/${id}`;
  };

  const registrarDeporte = async (event) => {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (!nombre.trim() || !descripcion.trim()) {
      setError("complete los campos para registrar un deporte");
      return;
    }

    setLoading(true);
    try {
      const response = await fetch("http://localhost:5266/api/deportes", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ nombre: nombre.trim(), descripcion: descripcion.trim() }),
      });

      const body = await response.json().catch(() => null);

      if (response.ok) {
        setSuccess("deporte registrado correctamente");
        setNombre("");
        setDescripcion("");
        cargarDeportes();
      } else {
        setError(body?.message ?? "Error al registrar el deporte. Intenta nuevamente.");
      }
    } catch (error) {
      console.error("Error al registrar deporte:", error);
      setError("Error al registrar el deporte. Intenta nuevamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>Registrar deporte</h1>
      <p>Agrega un nuevo deporte al sistema para que pueda ser seleccionado por los clientes.</p>

      <RegistrarDeporteForm
        nombre={nombre}
        setNombre={setNombre}
        descripcion={descripcion}
        setDescripcion={setDescripcion}
        onSubmit={registrarDeporte}
        loading={loading}
        error={error}
        success={success}
      />

      <BotonMostrarListadoDeportes onClick={cargarDeportes} />

      <ul>
        {deportes.map((d) => (
          <li key={d.id} style={{ marginBottom: "12px" }}>
            <strong>{d.nombre}</strong> - {d.descripcion}
            <BotonModificarDeporte onClick={() => modificarDeporte(d.id)} />
          </li>
        ))}
      </ul>
    </div>
  );
}

export default CrearDeportePage;
