import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import ModificarDeporteForm from "../components/FrontDeportes/ModificarDeporteForm";

function ModificarDeportePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [nombre, setNombre] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  const handleModificar = async (event) => {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (!nombre.trim() || !descripcion.trim()) {
      setError("Debes completar nombre y descripción antes de modificar.");
      return;
    }

    setLoading(true);

    try {
      const response = await fetch(`http://localhost:5266/api/deportes/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ nombre: nombre.trim(), descripcion: descripcion.trim() }),
      });

      if (!response.ok) {
        const body = await response.json().catch(() => null);
        setError(body?.message ?? "No se pudo modificar el deporte.");
        return;
      }

      setSuccess("Deporte modificado con éxito.");
      setTimeout(() => navigate("/deportes"), 1000);
    } catch (err) {
      console.error(err);
      setError("Error al modificar el deporte. Intenta nuevamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>Modificar Deporte</h1>
      <p>Ingresa el nuevo nombre y la nueva descripción del deporte.</p>

      <ModificarDeporteForm
        nombre={nombre}
        setNombre={setNombre}
        descripcion={descripcion}
        setDescripcion={setDescripcion}
        onSubmit={handleModificar}
        loading={loading}
        error={error}
        success={success}
      />

      <button type="button" onClick={() => navigate("/deportes")}>Volver a Deportes</button>
    </div>
  );
}

export default ModificarDeportePage;
