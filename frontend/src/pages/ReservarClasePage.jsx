import { useState, useEffect, useContext } from "react";
import { Link, useNavigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";

function ReservarClasePage() {
  const { user } = useContext(AuthContext);
  const navigate = useNavigate();
  const [turnos, setTurnos] = useState([]);
  const [turnoSeleccionado, setTurnoSeleccionado] = useState("");
  
  const [loading, setLoading] = useState(false);
  const [mensaje, setMensaje] = useState("");
  const [esError, setEsError] = useState(false);
  const [reservaExitosa, setReservaExitosa] = useState(false);
  const [requierePago, setRequierePago] = useState(false);

  useEffect(() => {
    if (!user) {
      alert("Debes iniciar sesión para reservar una clase.");
      navigate("/login");
      return;
    }
    if (user.esAdmin) {
      alert("Los administradores no pueden reservar clases.");
      navigate("/turnos");
      return;
    }
  }, [user, navigate]);

  useEffect(() => {
    // Cargar turnos disponibles
    const cargarTurnos = async () => {
      try {
        const response = await fetch("http://localhost:5266/api/Turnos");
        if (response.ok) {
          const data = await response.json();
          // Filtrar los que tienen cupo > 0
          setTurnos(data.filter(t => t.cupo > 0));
        }
      } catch (error) {
        console.error("Error al cargar turnos", error);
      }
    };
    cargarTurnos();
  }, []);

  const handleReservar = async (e) => {
    e.preventDefault();
    if (!turnoSeleccionado) {
      setMensaje("Por favor seleccioná un turno.");
      setEsError(true);
      return;
    }

    setLoading(true);
    setMensaje("");
    setReservaExitosa(false);
    setRequierePago(false);

    try {
      const response = await fetch("http://localhost:5266/api/Reservas/reservar-turno", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ 
            email: user.email, 
            idTurno: turnoSeleccionado 
        })
      });

      const data = await response.json();

      if (response.ok) {
        setEsError(false);
        setMensaje(data.mensaje);
        setReservaExitosa(true);
        
        if (data.mensaje === "Reserva casi lista!") {
            setRequierePago(true);
        }
      } else {
        setEsError(true);
        setMensaje(data.mensaje || "Ocurrió un error al intentar reservar.");
      }
    } catch (error) {
      setEsError(true);
      setMensaje("Error de conexión con el servidor.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>Reservar Clase Individual</h1>
      <p>Buscá una clase disponible y asegurá tu lugar.</p>
      
      <div style={{ maxWidth: "500px", margin: "0 auto", padding: "20px", border: "1px solid #ccc", borderRadius: "8px", backgroundColor: "#f9f9f9" }}>
        
        {!reservaExitosa ? (
            <form onSubmit={handleReservar} style={{ display: "flex", flexDirection: "column", gap: "15px" }}>
                <div>
                    <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold" }}>Clase / Turno</label>
                    <select 
                        value={turnoSeleccionado} 
                        onChange={(e) => setTurnoSeleccionado(e.target.value)}
                        style={{ width: "100%", padding: "8px", boxSizing: "border-box" }}
                        required
                    >
                        <option value="">-- Seleccioná un turno --</option>
                        {turnos.map(t => (
                            <option key={t.id} value={t.id}>
                                {t.nombreTurno} - {t.fecha.split('T')[0]} - Cupo: {t.cupo}
                            </option>
                        ))}
                    </select>
                </div>

                <button 
                    type="submit" 
                    disabled={loading}
                    style={{ padding: "10px", backgroundColor: "#007bff", color: "white", border: "none", borderRadius: "5px", cursor: loading ? "not-allowed" : "pointer" }}
                >
                    {loading ? "Procesando..." : "Reservar clase"}
                </button>
            </form>
        ) : (
            <div style={{ textAlign: "center", padding: "20px" }}>
                <h2 style={{ color: requierePago ? "#ff9800" : "green" }}>{mensaje}</h2>
                
                {requierePago && (
                    <div style={{ marginTop: "20px" }}>
                        <p>Para confirmar tu lugar, aboná la seña del 50%.</p>
                        <button style={{ padding: "10px 20px", backgroundColor: "#28a745", color: "white", border: "none", borderRadius: "5px", cursor: "pointer", fontSize: "16px" }}>
                            Proceder al pago
                        </button>
                    </div>
                )}
                
                <div style={{ marginTop: "30px" }}>
                    <Link to="/reservas" style={{ color: "#007bff", textDecoration: "none" }}>Ver mis reservas</Link>
                </div>
            </div>
        )}

        {mensaje && !reservaExitosa && (
            <div style={{ marginTop: "15px", padding: "10px", backgroundColor: esError ? "#f8d7da" : "#d4edda", color: esError ? "#721c24" : "#155724", borderRadius: "4px" }}>
                {mensaje}
            </div>
        )}
      </div>
    </div>
  );
}

export default ReservarClasePage;
