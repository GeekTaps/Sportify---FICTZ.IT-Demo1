import { useState, useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import logoIcono from "../assets/logo_dibujito_sportify.png";
import logoLetras from "../assets/logo_letras_sportify-no-bg.png";

function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const response = await fetch("http://localhost:5266/api/usuarios/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (!response.ok) {
        const contentType = response.headers.get("Content-Type") || "";
        let errorMessage = "Usuario o contraseña incorrectos";

        if (contentType.includes("application/json")) {
          const errorData = await response.json();
          errorMessage = errorData?.message || errorMessage;
        } else {
          const errorText = await response.text();
          errorMessage = errorText || errorMessage;
        }

        throw new Error(errorMessage);
      }

      const data = await response.json();
      login(data);
      navigate("/");
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-split-page">
      {/* Panel izquierdo */}
      <div className="auth-split-left">
        <img src={logoIcono} alt="" className="auth-split-logo-icon" />
        <img src={logoLetras} alt="Sportify" className="auth-split-letters" />
        <h2>Tu deporte, tu ritmo</h2>
        <p>
          Reservá clases, gestioná tus actividades y disfrutá al máximo de tu
          experiencia deportiva.
        </p>
      </div>

      {/* Panel derecho — formulario */}
      <div className="auth-split-right">
        <div className="auth-split-form">
          <h2>Iniciar Sesión</h2>
          <p className="auth-split-form-subtitle">Ingresá tus datos para continuar</p>

          <form onSubmit={handleLogin}>
            <div className="form-group">
              <label htmlFor="email">Email</label>
              <input
                id="email"
                type="email"
                placeholder="tu@email.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="password">Contraseña</label>
              <input
                id="password"
                type="password"
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>

            {error && <div className="alert alert-error">{error}</div>}

            <button
              type="submit"
              disabled={loading}
              className="btn btn-primary"
              style={{ width: "100%", marginTop: "0.25rem" }}
            >
              {loading ? "Iniciando..." : "Iniciar Sesión"}
            </button>
          </form>

          <div className="auth-split-form-footer">
            ¿No tenés cuenta?{" "}
            <Link to="/register">Registrate aquí</Link>
          </div>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
