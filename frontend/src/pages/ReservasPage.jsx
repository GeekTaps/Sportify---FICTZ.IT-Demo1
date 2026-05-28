import { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";

function ReservasPage() {
  const { user } = useContext(AuthContext);
  const [reservas, setReservas] = useState([]);
  const [mensaje, setMensaje] = useState("");
  
  // Estados para el Modal
  const [reservaSeleccionada, setReservaSeleccionada] = useState(null);
  const [loadingDetalles, setLoadingDetalles] = useState(false);
  const [errorModal, setErrorModal] = useState("");
  const [mensajeCancelacion, setMensajeCancelacion] = useState(null);
  const [cancelando, setCancelando] = useState(false);

  const cargarReservas = async () => {
    if (!user) {
      setMensaje("Debes iniciar sesión para ver tus reservas");
      return;
    }
    
    try {
      setMensaje("");
      const response = await fetch(`http://localhost:5266/api/Reservas/usuario/email/${user.email}`);
      
      if (!response.ok) {
        if (response.status === 404) {
           const errData = await response.json();
           throw new Error(errData.mensaje || "No se encontraron reservas para este usuario.");
        }
        throw new Error(`Error HTTP ${response.status}`);
      }
      
      const data = await response.json();
      setReservas(data);
      if (data.length === 0) {
        setMensaje("No hay reservas activas actualmente.");
      }
    } catch (error) {
      console.error("Error al cargar reservas:", error);
      setMensaje(error.message);
      setReservas([]);
    }
  };

  useEffect(() => {
    if (user) {
      cargarReservas();
    } else {
      setMensaje("Debes iniciar sesión para ver tus reservas");
    }
  }, [user]);

  const abrirModal = async (idReserva) => {
    setLoadingDetalles(true);
    setErrorModal("");
    setMensajeCancelacion(null);
    setReservaSeleccionada(null); // Abre el modal mostrando un "Cargando..."
    
    // Lo mostramos con un objeto vacío temporal para que aparezca la ventana
    setReservaSeleccionada({ isLoading: true });

    try {
      const res = await fetch(`http://localhost:5266/api/Reservas/${idReserva}/detalles`);
      if (!res.ok) throw new Error("Error al obtener los detalles de la reserva.");
      const data = await res.json();
      setReservaSeleccionada(data);
    } catch (err) {
      setErrorModal(err.message);
      setReservaSeleccionada({ error: err.message });
    } finally {
      setLoadingDetalles(false);
    }
  };

  const cerrarModal = () => {
    setReservaSeleccionada(null);
    setMensajeCancelacion(null);
    if (mensajeCancelacion) {
        // Si acabamos de cancelar una reserva, recargamos la lista
        cargarReservas();
    }
  };

  const handleCancelarReserva = async (idReserva) => {
    setCancelando(true);
    try {
        const response = await fetch(`http://localhost:5266/api/Reservas/${idReserva}/cancelar`, {
            method: 'POST'
        });
        const data = await response.json();
        
        if (response.ok) {
            setMensajeCancelacion({ tipo: "success", texto: data.mensaje });
        } else {
            setMensajeCancelacion({ tipo: "error", texto: data.mensaje || "Error al cancelar." });
        }
    } catch (err) {
        setMensajeCancelacion({ tipo: "error", texto: "Error de conexión." });
    } finally {
        setCancelando(false);
    }
  };

  return (
    <div>
      <h1>Mis Reservas</h1>
      

      {user?.esAdmin && (
        <div style={{ padding: "10px", background: "#ffeb3b", color: "black", marginBottom: "15px" }}>
          Advertencia: Eres administrador. Esta vista está pensada para clientes.
        </div>
      )}



      {mensaje && <p style={{ color: "red", fontWeight: "bold" }}>{mensaje}</p>}

      {reservas.length > 0 && (
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
          <ul style={{ listStyleType: "none", padding: 0, width: "100%", maxWidth: "600px" }}>
            {reservas.map((r) => (
              <li 
                key={r.id} 
                onClick={() => abrirModal(r.id)}
                style={{ 
                  marginBottom: "12px", 
                  border: "1px solid #ccc", 
                  padding: "15px", 
                  borderRadius: "8px",
                  textAlign: "left",
                  backgroundColor: "#f9f9f9",
                  cursor: "pointer",
                  transition: "background-color 0.2s"
                }}
                onMouseOver={(e) => e.currentTarget.style.backgroundColor = '#eaeaea'}
                onMouseOut={(e) => e.currentTarget.style.backgroundColor = '#f9f9f9'}
              >
                <h3 style={{ marginTop: 0, color: "#333", textDecoration: "underline" }}>{r.titulo}</h3>
                <strong>Estado de Pago:</strong> {r.paga ? "Pagada ✅" : "Pendiente de pago ❌"} <br/>
                <strong>Monto Total:</strong> ${r.monto}
              </li>
            ))}
          </ul>
        </div>
      )}

      {/* MODAL DE DETALLES */}
      {reservaSeleccionada && (
        <div style={{
          position: "fixed", top: 0, left: 0, right: 0, bottom: 0,
          backgroundColor: "rgba(0,0,0,0.5)",
          display: "flex", justifyContent: "center", alignItems: "center"
        }}>
          <div style={{
            background: "#fff", padding: "20px", borderRadius: "8px",
            width: "400px", maxWidth: "90%", textAlign: "left", boxShadow: "0 4px 6px rgba(0,0,0,0.1)"
          }}>
            {reservaSeleccionada.isLoading ? (
              <p>Cargando información...</p>
            ) : reservaSeleccionada.error ? (
              <div>
                <p style={{ color: "red" }}>{reservaSeleccionada.error}</p>
                <button onClick={cerrarModal}>Cerrar</button>
              </div>
            ) : (
              <div>
                <h2 style={{ marginTop: 0 }}>Detalle de Reserva</h2>
                <p><strong>Actividad:</strong> {reservaSeleccionada.actividad}</p>
                <p><strong>Fecha:</strong> {reservaSeleccionada.fecha}</p>
                <p><strong>Horario:</strong> {reservaSeleccionada.horario}</p>
                <p><strong>Profesor designado:</strong> {reservaSeleccionada.profesor}</p>
                
                {mensajeCancelacion ? (
                  <div style={{ marginTop: "15px", padding: "15px", backgroundColor: mensajeCancelacion.tipo === "success" ? "#d4edda" : "#f8d7da", color: mensajeCancelacion.tipo === "success" ? "#155724" : "#721c24", borderRadius: "5px", fontWeight: "bold" }}>
                    {mensajeCancelacion.texto}
                  </div>
                ) : (
                  <>
                    <hr style={{ margin: "15px 0" }} />
                    
                    {reservaSeleccionada.suspendido ? (
                      <p style={{ color: "darkred", fontWeight: "bold", border: "1px solid darkred", padding: "8px", borderRadius: "4px", backgroundColor: "#ffebee" }}>
                        🚫 Tu cuenta está suspendida. En caso de cancelar, no se devolverá el valor de la seña.
                      </p>
                    ) : (
                      <>
                        {/* Lógica de Antelación (> 48hs) */}
                        {reservaSeleccionada.horasAnticipacion > 48 ? (
                          <p style={{ color: "green", fontWeight: "bold" }}>
                            ℹ️ En caso de cancelar, se devolverá el valor completo de la seña.
                          </p>
                        ) : (
                          <p style={{ color: "red", fontWeight: "bold" }}>
                            ⚠️ En caso de cancelar, no se devolverá la seña.
                          </p>
                        )}

                        {/* Lógica de Cancelaciones en el mes (>= 2) */}
                        {reservaSeleccionada.cancelacionesMes >= 2 && (
                          <p style={{ color: "darkred", fontWeight: "bold", border: "1px solid darkred", padding: "8px", borderRadius: "4px", backgroundColor: "#ffebee" }}>
                            🛑 Ya contás con dos cancelaciones este mes. En caso de cancelar, tu cuenta será suspendida.
                          </p>
                        )}
                      </>
                    )}
                  </>
                )}

                <div style={{ display: "flex", justifyContent: "flex-end", gap: "10px", marginTop: "20px" }}>
                  <button onClick={cerrarModal} style={{ padding: "8px 16px", cursor: "pointer", background: "#ddd", border: "none", borderRadius: "4px" }}>
                    Cerrar
                  </button>
                  {!mensajeCancelacion && (
                      <button 
                        onClick={() => handleCancelarReserva(reservaSeleccionada.idReserva)} 
                        disabled={cancelando}
                        style={{ padding: "8px 16px", cursor: cancelando ? "not-allowed" : "pointer", background: "#d32f2f", color: "white", border: "none", borderRadius: "4px", opacity: cancelando ? 0.7 : 1 }}
                      >
                        {cancelando ? "Cancelando..." : "Cancelar reserva"}
                      </button>
                  )}
                </div>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
}

export default ReservasPage;
