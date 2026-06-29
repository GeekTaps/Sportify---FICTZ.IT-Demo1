import { useState } from "react";
import { useNavigate } from "react-router-dom";
import RegistrarDeporteForm from "../components/FrontDeportes/RegistrarDeporteForm";

function CrearDeportePage() {
  const navigate = useNavigate();

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

    if (!nombre.trim() || !descripcion.trim() || precio === "") {
      setError("Completá todos los campos.");
      return;
    }

    const precioNumerico = Number(precio);
    if (Number.isNaN(precioNumerico) || precioNumerico < 0) {
      setError("El precio debe ser un número mayor o igual a 0.");
      return;
    }

    setLoading(true);

    try {
      const response = await fetch("http://localhost:5266/api/deportes", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ nombre: nombre.trim(), descripcion: descripcion.trim(), precio: precioNumerico }),
      });

      if (!response.ok) {
        const body = await response.json().catch(() => null);
        setError(body?.message ?? "Error al registrar el deporte.");
        return;
      }

      setSuccess("Deporte registrado correctamente.");
      setNombre("");
      setDescripcion("");
      setPrecio("");
      setTimeout(() => navigate("/deportes"), 1000);
    } catch (err) {
      console.error(err);
      setError("Error al registrar el deporte.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <div className="page-header" style={{ textAlign: "left" }}>
        <h1>Registrar Deporte</h1>
        <p>Agregá una nueva actividad deportiva al catálogo.</p>
      </div>

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

      <button
        type="button"
        onClick={() => navigate("/deportes")}
        className="btn btn-outline"
        style={{ marginTop: "1rem" }}
      >
        ← Volver a Deportes
      </button>
    </div>
  );
}

export default CrearDeportePage;
