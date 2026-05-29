import BotonModificarDeporte from "../components/FrontDeportes/BotonModificarDeporte";
import BotonEliminarDeporte from "../components/FrontDeportes/BotonEliminarDeporte";
import { useState, useContext, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
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

  useEffect(() => {
    cargarDeportes();
  }, []);

  const modificarDeporte = (id) => {
    navigate(`/deportes/modificar/${id}`);
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
      <div className="page-header">
        <h1>Deportes Disponibles</h1>
      </div>

      {user?.esAdmin && (
        <div style={{ display: "flex", justifyContent: "center", marginBottom: "2rem" }}>
          <Link to="/deportes/crear">
            <button className="btn btn-primary">+ Crear Deporte</button>
          </Link>
        </div>
      )}

      {deportes.length === 0 ? (
        <p style={{ textAlign: "center", color: "var(--text-muted)" }}>
          Por el momento no hay deportes disponibles.
        </p>
      ) : (
        <ul className="grid-list">
          {deportes.map((d) => (
            <li key={d.id} className="card" style={{ display: "flex", flexDirection: "column", justifyContent: "space-between" }}>
              <div>
                <h3 style={{ marginTop: 0, color: "var(--primary)" }}>{d.nombre}</h3>
                <p style={{ color: "var(--text-muted)", marginBottom: "1.5rem" }}>{d.descripcion}</p>
              </div>
              {user?.esAdmin && (
                <div style={{ marginTop: "auto", display: "flex", justifyContent: "flex-end", gap: "0.5rem" }}>
                  <BotonModificarDeporte onClick={() => modificarDeporte(d.id)} />
                  <BotonEliminarDeporte onClick={() => eliminarDeporte(d.id)} />
                </div>
              )}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default DeportePage;
