import BotonModificarTurno from "../components/FrontTurnos/BotonModificarTurno";
import { useState, useContext, useEffect } from "react";
import { useNavigate, Link, useLocation } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import { initMercadoPago, Wallet } from '@mercadopago/sdk-react';

// Inicializar MercadoPago con la Public Key
initMercadoPago(import.meta.env.VITE_MP_PUBLIC_KEY, { locale: 'es-AR' });

function TurnoPage() {
  const { user } = useContext(AuthContext);
  const [turnos, setTurnos] = useState([]);
  const [modalTurno, setModalTurno] = useState(null);
  const [userInfo, setUserInfo] = useState(null);
  const [loadingUser, setLoadingUser] = useState(false);

  // Estados de reserva
  const [loadingReserva, setLoadingReserva] = useState(false);
  const [mensajeReserva, setMensajeReserva] = useState("");
  const [reservaExitosa, setReservaExitosa] = useState(false);
  const [requierePago, setRequierePago] = useState(false);
  const [esErrorReserva, setEsErrorReserva] = useState(false);
  const [preferenceId, setPreferenceId] = useState(null);

  const navigate = useNavigate();
  const location = useLocation();

  // Revisar si volvimos de Mercado Pago con error
  useEffect(() => {
    const params = new URLSearchParams(location.search);
    if (params.get("pago") === "rechazado") {
      alert("El pago no pudo completarse. Se ha cancelado la reserva y se restauró el cupo de la clase.");
      // Limpiar URL
      navigate("/turnos", { replace: true });
    }
  }, [location, navigate]);

  const cargarTurnos = async () => {
    try {
      const response = await fetch("http://localhost:5266/api/turnos");
      if (!response.ok) {
        throw new Error(`Error HTTP ${response.status}`);
      }
      const data = await response.json();
      setTurnos(data);
    } catch (error) {
      console.error("Error al cargar turnos:", error);
      alert("Error al cargar los turnos. Asegúrate de que el backend esté ejecutándose.");
    }
  };

  useEffect(() => {
    cargarTurnos();
  }, []);

  const modificarTurno = (id) => {
    navigate(`/turnos/modificar/${id}`);
  };

  const formatearHora = (horaString) => {
    if (!horaString) return "N/A";
    // Si es un string de tiempo HH:MM, devolverlo tal cual
    if (horaString.length === 5) return horaString;
    // Si es un objeto con horas y minutos
    if (typeof horaString === "object" && horaString.hours !== undefined) {
      return `${String(horaString.hours).padStart(2, "0")}:${String(horaString.minutes).padStart(2, "0")}`;
    }
    return horaString;
  };

  const formatearFecha = (fechaString) => {
    if (!fechaString) return "N/A";
    const fecha = new Date(fechaString);
    return fecha.toLocaleDateString("es-ES");
  };

  const abrirModal = async (turno) => {
    setModalTurno(turno);
    
    if (!user) {
      setUserInfo({ error: "No logueado" });
      return;
    }
    
    if (user.esAdmin) {
      setUserInfo({ esAdmin: true });
      return;
    }

    setLoadingUser(true);
    setUserInfo(null);
    setMensajeReserva("");
    setReservaExitosa(false);
    setRequierePago(false);
    setEsErrorReserva(false);

    try {
      const response = await fetch(`http://localhost:5266/api/usuarios/info/${encodeURIComponent(user.email)}`);
      if (response.ok) {
        const data = await response.json();
        setUserInfo(data);
      } else {
        setUserInfo({ error: "Usuario no encontrado" });
      }
    } catch (err) {
      setUserInfo({ error: "Error de red" });
    } finally {
      setLoadingUser(false);
    }
  };

  const cerrarModal = () => {
    setModalTurno(null);
    setUserInfo(null);
    setMensajeReserva("");
    setReservaExitosa(false);
    setRequierePago(false);
    setEsErrorReserva(false);
    setPreferenceId(null);
  };

  const handleReservar = async () => {
    setLoadingReserva(true);
    setMensajeReserva("");
    setReservaExitosa(false);
    setRequierePago(false);
    setEsErrorReserva(false);
    setPreferenceId(null);

    try {
      const response = await fetch("http://localhost:5266/api/Reservas/reservar-turno", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ 
            email: user.email, 
            idTurno: modalTurno.id 
        })
      });

      const data = await response.json();

      if (response.ok) {
        setEsErrorReserva(false);
        setMensajeReserva(data.mensaje);
        setReservaExitosa(true);
        
        if (data.mensaje === "Requiere pago") {
            setRequierePago(true);
            setMensajeReserva("Redirigiendo a Mercado Pago...");
            // Pedir al backend que cree la preferencia de Mercado Pago
            try {
              const pagoResponse = await fetch("http://localhost:5266/api/pagos/crear-preferencia", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                  idTurno: modalTurno.id,
                  email: user.email
                })
              });
              
              if (pagoResponse.ok) {
                const pagoData = await pagoResponse.json();
                setPreferenceId(pagoData.preferenceId);
              } else {
                const errorData = await pagoResponse.json();
                setMensajeReserva(`Error MP: ${errorData.message} - ${errorData.error || ''}`);
              }
            } catch (err) {
              console.error("Error al crear preferencia:", err);
              setMensajeReserva("Error al crear preferencia: " + err.message);
            }
        }
        // Actualizamos cupo localmente o recargamos
        cargarTurnos();
      } else {
        setEsErrorReserva(true);
        setMensajeReserva(data.mensaje || "Ocurrió un error al intentar reservar.");
      }
    } catch (error) {
      setEsErrorReserva(true);
      setMensajeReserva("Error de conexión con el servidor.");
    } finally {
      setLoadingReserva(false);
    }
  };

  return (
    <div>
      <h1>Gestión de Turnos</h1>
      <p>Administra los turnos disponibles en el sistema.</p>

      <div style={{ marginBottom: "20px" }}>
      </div>

      {turnos.length === 0 ? (
        <p>Por el momento no hay turnos disponibles</p>
      ) : (
        <ul style={{ listStyle: "none", padding: 0 }}>
          {turnos.map((turno) => (
            <li
              key={turno.id}
              style={{
                marginBottom: "15px",
                padding: "15px",
                border: "1px solid #ddd",
                borderRadius: "4px",
                backgroundColor: "#f9f9f9",
              }}
            >
              <div 
                style={{ cursor: "pointer", display: "flex", justifyContent: "space-between", alignItems: "center" }}
                onClick={() => abrirModal(turno)}
              >
                <strong>{turno.nombreTurno || "Sin título"}</strong>
              </div>
            </li>
          ))}
        </ul>
      )}

      {/* MODAL DEL TURNO */}
      {modalTurno && (
        <div style={{
          position: "fixed", top: 0, left: 0, right: 0, bottom: 0,
          backgroundColor: "rgba(0,0,0,0.5)",
          display: "flex", justifyContent: "center", alignItems: "center"
        }}>
          <div style={{
            background: "#fff", padding: "20px", borderRadius: "8px",
            width: "400px", maxWidth: "90%", textAlign: "left", boxShadow: "0 4px 6px rgba(0,0,0,0.1)"
          }}>
            <h2 style={{ marginTop: 0 }}>Detalles del Turno</h2>
            <p><strong>Actividad:</strong> {modalTurno.nombreTurno}</p>
            <p><strong>Fecha:</strong> {formatearFecha(modalTurno.fecha)}</p>
            <p><strong>Horario:</strong> {formatearHora(modalTurno.horaInicio)}</p>
            <p><strong>Profesor designado:</strong> {modalTurno.nommbreProfesor || "N/A"}</p>
            <p><strong>Precio:</strong> ${modalTurno.precio}</p>

            <hr style={{ margin: "15px 0" }} />

            {loadingUser ? (
              <p>Verificando datos de usuario...</p>
            ) : !user ? (
              <p style={{ color: "#856404", fontWeight: "bold", border: "1px solid #ffeeba", padding: "8px", borderRadius: "4px", backgroundColor: "#fff3cd" }}>
                Debes iniciar sesión para reservar.
              </p>
            ) : userInfo?.esAdmin ? (
              null
            ) : userInfo?.error ? (
              <p style={{ color: "red" }}>Error al verificar tu cuenta.</p>
            ) : userInfo?.suspendido ? (
              <p style={{ color: "darkred", fontWeight: "bold", border: "1px solid darkred", padding: "8px", borderRadius: "4px", backgroundColor: "#ffebee" }}>
                La cuenta se encuentra suspendida. No es posible reservar por el momento
              </p>
            ) : (
              // Usuario Activo
              <div>
                {!reservaExitosa ? (
                  <>
                    {modalTurno.cupo > 0 ? (
                      <button 
                        onClick={handleReservar} 
                        disabled={loadingReserva}
                        style={{ padding: "10px", background: loadingReserva ? "#ccc" : "#28a745", color: "white", border: "none", borderRadius: "4px", cursor: loadingReserva ? "not-allowed" : "pointer", width: "100%", fontWeight: "bold" }}
                      >
                        {loadingReserva ? "Procesando..." : "Reservar turno"}
                      </button>
                    ) : modalTurno.listaEsperaHabilitada ? (
                      <button onClick={() => alert("Función de lista de espera no implementada aún.")} style={{ padding: "10px", background: "#ffc107", color: "black", border: "none", borderRadius: "4px", cursor: "pointer", width: "100%", fontWeight: "bold" }}>
                        Entrar a lista de espera
                      </button>
                    ) : (
                      <p style={{ color: "#856404", backgroundColor: "#fff3cd", border: "1px solid #ffeeba", padding: "10px", borderRadius: "4px", fontWeight: "bold" }}>
                        Por el momento no hay más cupos para esta actividad
                      </p>
                    )}
                    
                    {mensajeReserva && (
                      <div style={{ marginTop: "15px", padding: "10px", backgroundColor: esErrorReserva ? "#f8d7da" : "#d4edda", color: esErrorReserva ? "#721c24" : "#155724", borderRadius: "4px" }}>
                        {mensajeReserva}
                      </div>
                    )}
                  </>
                ) : (
                  <div style={{ textAlign: "center", padding: "10px" }}>
                    <h3 style={{ color: requierePago ? "#ff9800" : "green", marginTop: 0 }}>{mensajeReserva}</h3>
                    
                    {requierePago && (
                        <div style={{ marginTop: "15px" }}>
                            <p>Para confirmar tu lugar, aboná la seña del 50%.</p>
                            {preferenceId ? (
                              <Wallet initialization={{ preferenceId: preferenceId }} customization={{ texts: { action: 'pay' } }} />
                            ) : (
                              <p>Cargando botón de pago...</p>
                            )}
                        </div>
                    )}
                    
                    <div style={{ marginTop: "20px" }}>
                        <Link to="/reservas" style={{ color: "#007bff", textDecoration: "none", fontWeight: "bold" }}>Ir a Mis Reservas</Link>
                    </div>
                  </div>
                )}
              </div>
            )}

            {user?.esAdmin && (
              <div style={{ marginTop: "15px" }}>
                <BotonModificarTurno onClick={() => modificarTurno(modalTurno.id)} />
              </div>
            )}

            <div style={{ display: "flex", justifyContent: "flex-end", marginTop: "20px" }}>
              <button onClick={cerrarModal} style={{ padding: "8px 16px", cursor: "pointer", background: "#ddd", border: "none", borderRadius: "4px" }}>
                Cerrar
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default TurnoPage;
