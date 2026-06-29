import { useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";

function ResetearContraseñaPage() {
  const [searchParams] = useSearchParams();
  const email = searchParams.get("email");
  const token = searchParams.get("token");

  const [newPassword, setNewPassword] = useState("");
  const [confirmar, setConfirmar] = useState("");
  const [mensaje, setMensaje] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    if (newPassword !== confirmar) {
      setError("Las contraseñas no coinciden.");
      return;
    }

    setLoading(true);
    try {
      const response = await fetch("http://localhost:5266/api/auth/reset-password", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, token, newPassword }),
      });

      const data = await response.json();

      if (!response.ok) {
        setError(data.message || "Ocurrió un error.");
        return;
      }

      setMensaje(data.message);
      setTimeout(() => navigate("/login"), 3000);
    } catch {
      setError("Ocurrió un error, intentá de nuevo.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-split-page">
      <div className="auth-split-right" style={{ margin: "auto" }}>
        <div className="auth-split-form">
          <h2>Nueva contraseña</h2>
          <p className="auth-split-form-subtitle">
            Ingresá tu nueva contraseña.
          </p>

          {!mensaje ? (
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label htmlFor="newPassword">Nueva contraseña</label>
                <input
                  id="newPassword"
                  type="password"
                  placeholder="••••••••"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="confirmar">Confirmar contraseña</label>
                <input
                  id="confirmar"
                  type="password"
                  placeholder="••••••••"
                  value={confirmar}
                  onChange={(e) => setConfirmar(e.target.value)}
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
                {loading ? "Guardando..." : "Cambiar contraseña"}
              </button>
            </form>
          ) : (
            <>
              <div className="alert alert-success">{mensaje}</div>
              <p style={{ fontSize: "0.875rem", marginTop: "0.5rem" }}>
                Redirigiendo al login...
              </p>
            </>
          )}
        </div>
      </div>
    </div>
  );
}

export default ResetearContraseñaPage;