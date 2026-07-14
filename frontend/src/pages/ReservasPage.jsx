import { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";
import GeneradorQR from "../components/GeneradorQr";

function ReservasPage() {
  const { user } = useContext(AuthContext);
  const [reservas, setReservas] = useState([]);
  const [mensaje, setMensaje] = useState("");

  const [reservaSeleccionada, setReservaSeleccionada] = useState(null);
  const [loadingDetalles, setLoadingDetalles] = useState(false);
  const [errorModal, setErrorModal] = useState("");
  const [mensajeCancelacion, setMensajeCancelacion] = useState(null);
  const [cancelando, setCancelando] = useState(false);
  const [mostrarQR, setMostrarQR] = useState(false);
  const [viendoHistorial, setViendoHistorial] = useState(false);

  const cargarReservasActivas = async () => {
    if (!user) {
      setMensaje("Debes iniciar sesión para ver tus reservas");
      return;
    }

    try {
      setMensaje("");
      const response = await fetch(
        `http://localhost:5266/api/Reservas/usuario/activas/${user.id}`
      );

      if (!response.ok) {
        if (response.status === 404) {
          const errData = await response.json();
          throw new Error(
            errData.mensaje || "No se encontraron reservas para este usuario."
          );
        }
        throw new Error(`Error HTTP ${response.status}`);
      }

      const data = await response.json();
      setReservas(data);
      if (data.length === 0) {
        setMensaje("No Cuenta Con Reservas Activas Actualmente");
      }
    } catch (error) {
      console.error("Error al cargar reservas:", error);
      setMensaje(error.message);
      setReservas([]);
    }
  };

  const cargarReservas = async () => {
    if (!user) {
      setMensaje("Debes iniciar sesión para ver tus reservas");
      return;
    }

    try {
      setMensaje("");
      const response = await fetch(
        `http://localhost:5266/api/Reservas/usuario/${user.id}`
      );

      if (!response.ok) {
        if (response.status === 404) {
          const errData = await response.json();
          throw new Error(
            errData.mensaje || "No se encontraron reservas para este usuario."
          );
        }
        throw new Error(`Error HTTP ${response.status}`);
      }

      const data = await response.json();
      setReservas(data);
      if (data.length === 0) {
        setMensaje("No Cuenta Con Reservas Activas Actualmente");
      }
    } catch (error) {
      console.error("Error al cargar reservas:", error);
      setMensaje(error.message);
      setReservas([]);
    }
  };

  const cargarReservasAnteriores = async () => {
    if (!user) {
      setMensaje("Debes iniciar sesión para ver tus reservas");
      return;
    }

    try {
      setMensaje("");
      const response = await fetch(
        `http://localhost:5266/api/Reservas/usuario/anteriores/${user.id}`
      );

      if (!response.ok) {
        if (response.status === 404) {
          const errData = await response.json();
          throw new Error(
            errData.mensaje || "No se encontraron reservas para este usuario."
          );
        }
        throw new Error(`Error HTTP ${response.status}`);
      }

      const data = await response.json();
      setReservas(data);
      if (data.length === 0) {
        setMensaje("No Cuenta Con Reservas Anteriores");
      }
    } catch (error) {
      console.error("Error al cargar reservas:", error);
      setMensaje(error.message);
      setReservas([]);
    }
  };

  useEffect(() => {
    if (user) {
      if (viendoHistorial) {
        cargarReservasAnteriores();
      } else {
        cargarReservasActivas();
      }
    } else {
      setMensaje("Debes iniciar sesión para ver tus reservas");
    }
  }, [user, viendoHistorial]);

  const abrirModal = async (idReserva) => {
    setLoadingDetalles(true);
    setErrorModal("");
    setMensajeCancelacion(null);
    setReservaSeleccionada({ isLoading: true });

    try {
      const res = await fetch(
        `http://localhost:5266/api/Reservas/${idReserva}/detalles`
      );
      if (!res.ok) throw new Error("Error al obtener los detalles de la reserva.");
      const data = await res.json();
      console.log("JSON que viene de .NET:", data);
      setReservaSeleccionada(data);
    } catch (err) {
      setErrorModal(err.message);
      setReservaSeleccionada({ error: err.message });
    } finally {
      setLoadingDetalles(false);
    }
  };

  const cerrarModal = () => {
    const huboCancel = !!mensajeCancelacion;
    setReservaSeleccionada(null);
    setMensajeCancelacion(null);
    setMostrarQR(false);
    if (huboCancel) {
      viendoHistorial ? cargarReservasAnteriores() : cargarReservasActivas();
    }
  };

  const handleCancelarReserva = async (idReserva) => {
    if (!window.confirm("¿Estás seguro de que deseas cancelar esta reserva?")) {
      return;
    }
    setCancelando(true);
    try {
      const response = await fetch(
        `http://localhost:5266/api/Reservas/${idReserva}/cancelar`,
        { method: "POST" }
      );
      const data = await response.json();

      if (response.ok) {
        setMensajeCancelacion({
          tipo: "success",
          texto: data.mensaje,
          advertencia: data.advertencia
        });
        viendoHistorial ? await cargarReservasAnteriores() : await cargarReservasActivas();
      } else {
        setMensajeCancelacion({
          tipo: "error",
          texto: data.mensaje || "Error al cancelar.",
        });
      }
    } catch (err) {
      setMensajeCancelacion({ tipo: "error", texto: "Error de conexión." });
    } finally {
      setCancelando(false);
    }
  };

  return (
    <div>
      <div className="page-header">
        <h1>{viendoHistorial ? "Historial de Reservas" : "Mis Reservas"}</h1>
        {/* <p>{viendoHistorial ? "Visualizá tus reservas pasadas o canceladas." : "Visualizá y gestioná tus reservas activas."}</p> */}
        {user && !user.esAdmin && (
          <div style={{ display: "flex", justifyContent: "center", gap: "10px", marginTop: "10px" }}>
            <button
              className={`btn ${!viendoHistorial ? 'btn-primary' : 'btn-secondary'}`}
              onClick={() => setViendoHistorial(false)}
            >
              Ver reservas actuales
            </button>
            <button
              className={`btn ${viendoHistorial ? 'btn-primary' : 'btn-secondary'}`}
              onClick={() => setViendoHistorial(true)}
            >
              Ver historial de reservas
            </button>
          </div>
        )}
      </div>

      {user?.esAdmin && (
        <div className="admin-warning">
          Sos administrador. Esta vista está pensada para clientes.
        </div>
      )}

      {mensaje && (
        <div
          className="alert alert-warning"
          style={{ maxWidth: "700px", margin: "0 auto 1.5rem" }}
        >
          {mensaje}
        </div>
      )}

      {reservas.length > 0 && (
        <ul className="reserva-list">
          {reservas.map((r) => (
            <li
              key={r.id}
              className="reserva-card"
              onClick={() => abrirModal(r.id)}
            >
              <div className="reserva-card-info">
                <h3>{r.titulo}</h3>
                <p>
                  <strong>Pago:</strong>{" "}
                  {r.paga ? "Confirmado ✅" : r.pagoSeña ? "Seña pagada 💰" : "Pendiente ⏳"}
                  <strong>Monto:</strong> ${r.monto}
                  {viendoHistorial && (
                    <> &nbsp;·&nbsp; <strong>Asistencia:</strong> {r.asistio ? "Presente ✅" : "Ausente ❌"} </>
                  )}
                </p>
              </div>
              <span className="reserva-card-arrow">›</span>
            </li>
          ))}
        </ul>
      )}

      {/* MODAL DE DETALLES */}
      {reservaSeleccionada && (
        <div className="modal-overlay" onClick={cerrarModal}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            {reservaSeleccionada.isLoading ? (
              <p>Cargando información...</p>
            ) : reservaSeleccionada.error ? (
              <div>
                <div className="alert alert-error">
                  {reservaSeleccionada.error}
                </div>
                <button
                  onClick={cerrarModal}
                  className="btn"
                  style={{ background: "var(--border)", color: "var(--text-main)" }}
                >
                  Cerrar
                </button>
              </div>
            ) : (
              <div>
                  {console.log(reservaSeleccionada)}
                <h2 style={{ marginTop: 0 }}>Detalle de Reserva</h2>
                <p>
                  <strong>Actividad:</strong> {reservaSeleccionada.actividad}
                </p>
                <p>
                  <strong>Fecha:</strong> {reservaSeleccionada.fecha}
                </p>
                <p>
                  <strong>Horario:</strong> {reservaSeleccionada.horario}
                </p>
                <p>
                  <strong>Profesor designado:</strong>{" "}
                  {reservaSeleccionada.profesor}
                </p>
                <p>
                 {reservaSeleccionada.paga ? "Confirmado ✅" : reservaSeleccionada.pagoSeña ? "Seña pagada 💰" : "Pendiente ⏳"}
                </p>
                <p>
                  <strong>Monto:</strong> ${reservaSeleccionada.monto}
                </p>
                {viendoHistorial && (
                  <p>
                    <strong>Asistencia:</strong> {reservaSeleccionada.asistio ? "Presente ✅" : "Ausente ❌"}
                  </p>
                )}
                {!mensajeCancelacion && reservaSeleccionada.horasAnticipacion >= 0 && (
                  <div style={{ marginTop: '1rem', textAlign: 'center' }}>
                    {!mostrarQR ? (
                      <button
                        onClick={() => setMostrarQR(true)}
                        className="btn"
                        style={{
                          background: '#0d47a1', // Un azul más intenso/oscuro que el original
                          color: 'white',
                          width: '100%',
                          padding: '18px 30px',  // Más padding para darle altura y presencia
                          fontSize: '1.25rem',   // Texto más grande
                          fontWeight: 'bold',    // Texto en negrita
                          borderRadius: '16px',  // Bordes bien redondeados
                          border: 'none',
                          cursor: 'pointer',
                          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)' // Una sutil sombra
                        }}
                      >
                        Mostrar QR de Asistencia
                      </button>
                    ) : (
                      <div style={{ animation: 'fadeIn 0.3s ease-in-out' }}>
                        <GeneradorQR idTurno={reservaSeleccionada.idTurno} />
                        <button
                          onClick={() => setMostrarQR(false)}
                          style={{ background: 'none', border: 'none', color: '#666', textDecoration: 'underline', cursor: 'pointer', marginTop: '5px', fontSize: '0.85rem' }}
                        >
                          Ocultar QR
                        </button>
                      </div>
                    )}
                  </div>
                )}

                {mensajeCancelacion ? (
                  <div style={{ marginTop: "1rem" }}>
                    <div
                      className={`alert ${mensajeCancelacion.tipo === "success"
                        ? "alert-success"
                        : "alert-error"
                        }`}
                    >
                      {mensajeCancelacion.texto}
                    </div>
                    {mensajeCancelacion.advertencia && (
                      <div className="alert alert-error" style={{ marginTop: "0.5rem" }}>
                        {mensajeCancelacion.advertencia}
                      </div>
                    )}
                  </div>
                ) : (
                  <>
                    <hr />

                    {reservaSeleccionada.horasAnticipacion >= 0 && (
                      <>
                        {reservaSeleccionada.suspendido ? (
                          <div className="alert alert-error">
                            Tu cuenta está suspendida. En caso de cancelar, no se
                            devolverá el valor de la seña.
                          </div>
                        ) : (
                          <>
                            {reservaSeleccionada.horasAnticipacion > 48 ? (
                              <div className="alert alert-success">
                                En caso de cancelar, se devolverá el valor completo de la seña.
                              </div>
                            ) : (
                              <div className="alert alert-warning">
                                En caso de cancelar, no se devolverá la seña.
                              </div>
                            )}

                            {reservaSeleccionada.cancelacionesMes >= 2 && (
                              <div className="alert alert-error">
                                Ya contás con dos cancelaciones este mes. En caso de
                                cancelar, tu cuenta será suspendida.
                              </div>
                            )}
                          </>
                        )}
                      </>
                    )}
                  </>
                )}

                <div
                  style={{
                    display: "flex",
                    justifyContent: "flex-end",
                    gap: "0.75rem",
                    marginTop: "1.5rem",
                  }}
                >
                  <button
                    onClick={cerrarModal}
                    className="btn"
                    style={{ background: "var(--border)", color: "var(--text-main)" }}
                  >
                    Cerrar
                  </button>
                  {!mensajeCancelacion && reservaSeleccionada.horasAnticipacion >= 0 && (
                    <button
                      onClick={() =>
                        handleCancelarReserva(reservaSeleccionada.idReserva)
                      }
                      disabled={cancelando}
                      className="btn btn-danger"
                    >
                      {cancelando ? "Cancelando..." : "Cancelar reserva"}
                    </button>
                  )}
                </div>
              </div>
            )}
          </div>
        </div>
      )
      }
    </div >
  );
}

export default ReservasPage;
