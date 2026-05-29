import { Link } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";

function HomePage() {
  const { user } = useContext(AuthContext);

  return (
    <section className="hero">
      <span className="hero-eyebrow">Centro Deportivo</span>

      <h1>
        ¡Bienvenido a{" "}
        <span className="gradient-text">Sportify</span>!
      </h1>

      <p className="hero-subtitle">
        La plataforma definitiva para organizar, reservar y disfrutar de tus
        actividades deportivas favoritas.
      </p>

      <hr className="hero-divider" />

      <div className="hero-actions">
        <Link to="/deportes">
          <button className="btn btn-primary" style={{ fontSize: "1rem", padding: "0.875rem 2.25rem" }}>
            Explorar Deportes
          </button>
        </Link>

        {!user ? (
          <Link to="/register">
            <button className="btn btn-secondary" style={{ fontSize: "1rem", padding: "0.875rem 2.25rem" }}>
              Registrarse ahora
            </button>
          </Link>
        ) : !user.esAdmin ? (
          <Link to="/reservas">
            <button className="btn btn-secondary" style={{ fontSize: "1rem", padding: "0.875rem 2.25rem" }}>
              Mis Reservas
            </button>
          </Link>
        ) : (
          <Link to="/turnos">
            <button className="btn btn-secondary" style={{ fontSize: "1rem", padding: "0.875rem 2.25rem" }}>
              Gestión de Turnos
            </button>
          </Link>
        )}
      </div>
    </section>
  );
}

export default HomePage;
