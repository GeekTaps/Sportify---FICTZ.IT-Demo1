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
          setMensajeReserva("Reserva casi lista!");
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
      <div className="page-header">
        <h1>Gestión de Turnos</h1>
      </div>

      {user?.esAdmin && (
        <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '2rem' }}>
          <Link to="/turnos/crear">
            <button className="btn btn-primary">+ Crear Turno</button>
          </Link>
        </div>
      )}

      {turnos.length === 0 ? (
        <p>Por el momento no hay turnos disponibles</p>
      ) : (
        <ul className="grid-list">
          {turnos.map((turno) => (
            <li
              key={turno.id}
              className="card"
              style={{ cursor: "pointer", transition: "all 0.2s" }}
              onClick={() => abrirModal(turno)}
            >
              <h3 style={{ marginTop: 0, color: "var(--primary)" }}>{turno.nombreTurno || "Sin título"}</h3>
              <p style={{ color: "var(--text-muted)" }}>
                Click para ver detalles y reservar
              </p>
            </li>
          ))}
        </ul>
      )}

      {/* MODAL DEL TURNO */}
      {modalTurno && (
        <div className="modal-overlay" onClick={cerrarModal}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <h2 style={{ marginTop: 0, color: "var(--c-azul-cobalto)" }}>Detalles del Turno</h2>
            <p><strong>Actividad:</strong> {modalTurno.nombreTurno}</p>
            <p><strong>Fecha:</strong> {formatearFecha(modalTurno.fecha)}</p>
            <p><strong>Horario:</strong> {formatearHora(modalTurno.horaInicio)}</p>
            <p><strong>Profesor designado:</strong> {modalTurno.nommbreProfesor || "N/A"}</p>
            <p><strong>Precio:</strong> ${modalTurno.precio}</p>

            <hr style={{ margin: "15px 0" }} />

            {loadingUser ? (
              <p>Verificando datos de usuario...</p>
            ) : !user ? (
              <div className="alert alert-warning">
                Debes iniciar sesión para reservar.{" "}
                <Link to="/login" style={{ color: "inherit", fontWeight: 700 }}>Iniciar sesión →</Link>
              </div>
            ) : userInfo?.esAdmin ? (
              null
            ) : userInfo?.error ? (
              <p style={{ color: "red" }}>Error al verificar tu cuenta.</p>
            ) : userInfo?.suspendido ? (
              <div className="alert alert-error">
                La cuenta se encuentra suspendida. No es posible reservar por el momento.
              </div>
            ) : (
              // Usuario Activo
              <div>
                {!reservaExitosa ? (
                  <>
                    {modalTurno.cupo > 0 ? (
                      <button
                        onClick={handleReservar}
                        disabled={loadingReserva}
                        className="btn btn-primary"
                        style={{ width: "100%" }}
                      >
                        {loadingReserva ? "Procesando..." : "Reservar turno"}
                      </button>
                    ) : /* modalTurno.listaEsperaHabilitada ? (
                      <button onClick={() => alert("Función de lista de espera no implementada aún.")} className="btn btn-secondary" style={{ width: "100%" }}>
                        Entrar a lista de espera
                      </button>
                    ) : */ (
                        <div className="alert alert-warning">
                          Por el momento no hay más cupos para esta actividad
                        </div>
                      )}

                    {mensajeReserva && (
                      <div className={`alert ${esErrorReserva ? 'alert-error' : 'alert-success'}`} style={{ marginTop: "15px" }}>
                        {mensajeReserva}
                      </div>
                    )}
                  </>
                ) : (
                  <div style={{ textAlign: "center", padding: "10px" }}>
                    <h3 style={{ color: requierePago ? "var(--c-azul-medio)" : "#065f46", marginTop: 0 }}>{mensajeReserva}</h3>

                    {requierePago && (
                      <div style={{ marginTop: "15px" }}>
                        <p>Para confirmar tu lugar, aboná la seña del 50%.</p>
                        {preferenceId ? (
                          <Wallet initialization={{ preferenceId: preferenceId }} customization={{ texts: { action: 'pay' } }} />
                        ) : (
                          <p style={{ color: "var(--text-muted)" }}>Cargando botón de pago...</p>
                        )}
                      </div>
                    )}

                    <div style={{ marginTop: "20px" }}>
                      <Link to="/reservas" style={{ color: "var(--primary)", fontWeight: "bold" }}>Ir a Mis Reservas</Link>
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
              <button onClick={cerrarModal} className="btn" style={{ background: "var(--border)", color: "var(--text-main)" }}>
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
