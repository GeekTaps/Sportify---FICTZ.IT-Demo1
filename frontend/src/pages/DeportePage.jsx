import BotonModificarDeporte from "../components/FrontDeportes/BotonModificarDeporte";
import BotonMostrarListadoDeportes from "../components/FrontDeportes/BotonMostrarListadoDeportes";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

function DeportePage() {
  const [deportes, setDeportes] = useState([]);
  const navigate = useNavigate();

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
    navigate(`/deportes/modificar/${id}`);
  };

  const irACrearDeporte = () => {
    navigate("/deportes/crear");
  };

  return (
    <div>
      <h1>Bienvenido a la página de deportes!</h1>
      <p>Explora los diferentes deportes disponibles.</p>

      <button type="button" onClick={irACrearDeporte}>
        Ir a Crear Deporte
      </button>

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

export default DeportePage;