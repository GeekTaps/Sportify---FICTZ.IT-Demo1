import { Link } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import logoDibujito from "../assets/logo_dibujito_sportify.png";

function HomePage() {
  const { user } = useContext(AuthContext);

  return (
    <div style={{
      display: "flex",
      flexDirection: "column",
      alignItems: "center",
      justifyContent: "center",
      textAlign: "center",
      minHeight: "70vh",
      padding: "2rem"
    }}>
      <img 
        src={logoDibujito} 
        alt="Sportify Logo" 
        style={{ width: "150px", marginBottom: "2rem", filter: "drop-shadow(0px 4px 6px rgba(0,0,0,0.1))" }} 
      />
      
      <h1 style={{ fontSize: "3.5rem", marginBottom: "1rem", color: "var(--c-azul-cobalto)" }}>
        ¡Bienvenido a <span style={{ color: "var(--c-cian-brillante)" }}>Sportify</span>!
      </h1>
      
      <p style={{ fontSize: "1.25rem", color: "var(--text-muted)", maxWidth: "600px", marginBottom: "3rem" }}>
        La plataforma definitiva para organizar, reservar y disfrutar de tus actividades deportivas favoritas.
      </p>

      <div style={{ display: "flex", gap: "1rem", justifyContent: "center", flexWrap: "wrap" }}>
        <Link to="/deportes">
          <button className="btn btn-primary" style={{ fontSize: "1.1rem", padding: "1rem 2rem" }}>Explorar Deportes</button>
        </Link>

        {!user ? (
          <>
            <Link to="/register">
              <button className="btn btn-secondary" style={{ fontSize: "1.1rem", padding: "1rem 2rem" }}>Registrarse ahora</button>
            </Link>
          </>
        ) : !user.esAdmin ? (
          <Link to="/reservas">
            <button className="btn btn-secondary" style={{ fontSize: "1.1rem", padding: "1rem 2rem" }}>Mis Reservas</button>
          </Link>
        ) : (
          <Link to="/turnos">
            <button className="btn btn-secondary" style={{ fontSize: "1.1rem", padding: "1rem 2rem" }}>Gestión de Turnos</button>
          </Link>
        )}
      </div>
      
      {/* Decorative shapes or patterns could go here */}
    </div>
  );
}

export default HomePage;