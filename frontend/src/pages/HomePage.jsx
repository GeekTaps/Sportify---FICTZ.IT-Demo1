import { Link } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";

function HomePage() {
  const { user } = useContext(AuthContext);

  return (
    <>
      <div>
        <h1>Bienvenido a Sportify!</h1>

        <div style={{ display: "flex", gap: "10px", justifyContent: "center" }}>
          <Link to="/deportes">
            <button>Ir a Deportes</button>
          </Link>

          {!user ? (
            <>
              <Link to="/login">
                <button>Iniciar Sesión</button>
              </Link>
              <Link to="/register">
                <button>Registrarse</button>
              </Link>
            </>
          ) : !user.esAdmin ? (
            <Link to="/reservas">
              <button>Mis Reservas</button>
            </Link>
          ) : null}
        </div>
      </div>
    </>
  );
}

export default HomePage;