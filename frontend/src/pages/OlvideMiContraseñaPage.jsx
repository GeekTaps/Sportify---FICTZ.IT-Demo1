import { useState } from "react";
import { Link } from "react-router-dom";

function OlvideMiContraseñaPage() {
  const [email, setEmail] = useState("");
  const [mensaje, setMensaje] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const response = await fetch("http://localhost:5266/api/auth/forgot-password", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email }),
      });

      const data = await response.json();
      setMensaje(data.message);
    } catch {
      setMensaje("Ocurrió un error, intentá de nuevo.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-split-page">
      <div className="auth-split-right" style={{ margin: "auto" }}>
        <div className="auth-split-form">
          <h2>Recuperar contraseña</h2>
          <p className="auth-split-form-subtitle">
            Ingresá tu email y te mandamos un link para resetear tu contraseña.
          </p>

          {!mensaje ? (
            <form onSubmit={handleSubmit}>
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

              <button
                type="submit"
                disabled={loading}
                className="btn btn-primary"
                style={{ width: "100%", marginTop: "0.25rem" }}
              >
                {loading ? "Enviando..." : "Enviar link"}
              </button>
            </form>
          ) : (
            <div className="alert alert-success">{mensaje}</div>
          )}

          <div className="auth-split-form-footer">
            <Link to="/login">Volver al inicio de sesión</Link>
          </div>
        </div>
      </div>
    </div>
  );
}

export default OlvideMiContraseñaPage;