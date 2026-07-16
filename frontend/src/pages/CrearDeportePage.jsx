import { useState } from "react";
import RegistrarDeporteForm from "../components/FrontDeportes/RegistrarDeporteForm";

function CrearDeportePage() {
  const [nombre, setNombre] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [precio, setPrecio] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);



  const registrarDeporte = async (event) => {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (!nombre.trim() || !descripcion.trim() || !precio.toString().trim()) {
      setError("Completá todos los campos");
      return;
    }

    if (Number(precio) < 1) {
      setError("El precio debe ser positivo");
      return;
    }

    setLoading(true);
    try {
      const response = await fetch("http://localhost:5266/api/deportes", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ nombre: nombre.trim(), descripcion: descripcion.trim(), precio: precio.trim() }),
      });

      const body = await response.json().catch(() => null);

      if (response.ok) {
        setSuccess("deporte registrado correctamente");
        setNombre("");
        setDescripcion("");
        setPrecio("");
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
        precio={precio}
        setPrecio={setPrecio}
        onSubmit={registrarDeporte}
        loading={loading}
        error={error}
        success={success}
      />

    </div>
  );
}

export default CrearDeportePage;
