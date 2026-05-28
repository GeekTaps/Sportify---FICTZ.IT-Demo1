import BotonModificarDeporte from "../components/FrontDeportes/BotonModificarDeporte";
import BotonMostrarListadoDeportes from "../components/FrontDeportes/BotonMostrarListadoDeportes";
import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";

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

  const modificarDeporte = (id) => {
    navigate(`/deportes/modificar/${id}`);
  };

  return (
    <div>
      <div className="page-header">
        <h1>Deportes Disponibles</h1>
        <p>Explora los diferentes deportes que ofrecemos y anótate en el que más te guste.</p>
      </div>

      <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '2rem' }}>
        <BotonMostrarListadoDeportes onClick={cargarDeportes} />
      </div>

      <ul className="grid-list">
        {deportes.map((d) => (
          <li key={d.id} className="card" style={{ display: "flex", flexDirection: "column", justifyContent: "space-between" }}>
            <div>
              <h3 style={{ marginTop: 0, color: "var(--primary)" }}>{d.nombre}</h3>
              <p style={{ color: "var(--text-muted)", marginBottom: "1.5rem" }}>{d.descripcion}</p>
            </div>
            {user?.esAdmin && (
              <div style={{ marginTop: "auto", display: "flex", justifyContent: "flex-end" }}>
                <BotonModificarDeporte onClick={() => modificarDeporte(d.id)} />
              </div>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default DeportePage;