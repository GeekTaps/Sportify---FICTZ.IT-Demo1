import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import ModificarDeporteForm from "../components/FrontDeportes/ModificarDeporteForm";

function ModificarDeportePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [nombre, setNombre] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [precio, setPrecio] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);
  const [fetchError, setFetchError] = useState("");

  useEffect(() => {
    async function cargarDeporte() {
      setFetchError("");

      try {
        const response = await fetch(`http://localhost:5266/api/deportes/${id}`);
        if (!response.ok) {
          const body = await response.json().catch(() => null);
          setFetchError(body?.message ?? "No se pudo cargar el deporte.");
          return;
        }

        const data = await response.json();
        setNombre(data.nombre ?? "");
        setDescripcion(data.descripcion ?? "");
        setPrecio(data.precio ?? "");
      } catch (err) {
        console.error(err);
        setFetchError("Error al cargar el deporte. Intenta nuevamente.");
      }
    }

    if (id) {
      cargarDeporte();
    }
  }, [id]);

  const handleModificar = async (event) => {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (fetchError) {
      setError("No se puede modificar hasta que se cargue el deporte correctamente.");
      return;
    }

    if (!descripcion.trim()) {
      setError("Debes completar la descripción antes de modificar.");
      return;
    }

    const precioNumerico = Number(precio);
    if (Number.isNaN(precioNumerico) || precioNumerico < 0) {
      setError("El precio debe ser un número mayor o igual a 0.");
      return;
    }

    setLoading(true);

    try {
      const response = await fetch(`http://localhost:5266/api/deportes/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ nombre: nombre.trim(), descripcion: descripcion.trim(), precio: precioNumerico }),
      });

      if (!response.ok) {
        const body = await response.json().catch(() => null);
        setError(body?.message ?? "No se pudo modificar el deporte.");
        return;
      }

      setSuccess("Modificación exitosa");
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
      <div className="page-header" style={{ textAlign: "left" }}>
        <h1>Modificar Deporte</h1>
        <p>Solo se puede cambiar la descripción del deporte. El nombre no es editable.</p>
      </div>

      {fetchError && <div className="alert alert-error">{fetchError}</div>}

      <ModificarDeporteForm
        nombre={nombre}
        setNombre={setNombre}
        descripcion={descripcion}
        setDescripcion={setDescripcion}
        precio={precio}
        setPrecio={setPrecio}
        onSubmit={handleModificar}
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

export default ModificarDeportePage;
