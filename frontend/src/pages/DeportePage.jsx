import BotonEliminarDeporte from "../components/FrontDeportes/BotonEliminarDeporte";
import BotonModificarDeporte from "../components/FrontDeportes/BotonModificarDeporte";
import { useState, useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import BotonMostrarListadoDeportes from "../components/FrontDeportes/BotonMostrarListadoDeportes";
function DeportePage() {
  const { user } = useContext(AuthContext);
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

  useEffect(() => {
    cargarDeportes();
  }, []);

  const modificarDeporte = (id) => {
    navigate(`/deportes/modificar/${id}`);
  };
  const irACrearDeporte = () => {

    navigate("/deportes/crear");

  };
  const eliminarDeporte = async (id) => {
    if (!window.confirm("¿Estás seguro de que deseas eliminar este deporte?")) {
      return;
    }
    
    try {
      const response = await fetch(`http://localhost:5266/api/deportes/${id}`, {
        method: "DELETE",
      });
      if (!response.ok) {
        throw new Error(`Error HTTP ${response.status}`);
      }
      setDeportes(deportes.filter((d) => d.id !== id));
    } catch (error) {
      console.error("Error al eliminar deporte:", error);
    }
  };
  

  return (
    <div>
      <h1>Bienvenido a la página de deportes!</h1>
      <p>Explora los diferentes deportes disponibles.</p>

      {user?.esAdmin && (
  <button type="button" onClick={irACrearDeporte}>
    Ir a Crear Deporte
  </button>
)}

      <BotonMostrarListadoDeportes onClick={cargarDeportes} />

      <ul>
        {deportes.map((d) => (
          <li key={d.id} style={{ marginBottom: "12px" }}>
            <strong>{d.nombre}</strong> - {d.descripcion}
            {user?.esAdmin && <BotonModificarDeporte onClick={() => modificarDeporte(d.id)} />}
            {user?.esAdmin && <BotonEliminarDeporte onClick={() => eliminarDeporte(d.id)} />}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default DeportePage;