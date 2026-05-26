import { Link } from "react-router-dom";

function HomePage() {
  return (
    <>
      <div>
        <h1>Bienvenido a Sportify!</h1>

        <Link to="/deportes">
          <button>Ir a Deportes</button>
        </Link>
      </div>
    </>
  );
}

export default HomePage;