import BotonModificarTurno from "../components/FrontTurnos/BotonModificarTurno";
import { useState, useContext, useEffect } from "react";
import { useNavigate, Link, useLocation } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
// HARDCODEO DE PAGOS: se deja comentado el bloque original de Mercado Pago como referencia.
// import { initMercadoPago, Wallet } from '@mercadopago/sdk-react';
import BotonCancelarTurno from "../components/FrontTurnos/BotonCancelarTurno";
import AbonoInfoModal from "../components/FrontAbonos/AbonoInfoModal";

// HARDCODEO DE PAGOS: se deja comentado el init original de Mercado Pago.
// initMercadoPago(import.meta.env.VITE_MP_PUBLIC_KEY, { locale: 'es-AR' });

function TurnoPage() {
  const { user } = useContext(AuthContext);
  const [turnos, setTurnos] = useState([]);
  const [modalTurno, setModalTurno] = useState(null);
  const [userInfo, setUserInfo] = useState(null);
  const [loadingUser, setLoadingUser] = useState(false);
  const [viendoHistorial, setViendoHistorial] = useState(false);
  const [infoHistoricaTurno, setInfoHistoricaTurno] = useState(null);

  // Estados de reserva
  const [loadingReserva, setLoadingReserva] = useState(false);
  const [mensajeReserva, setMensajeReserva] = useState("");
  const [reservaExitosa, setReservaExitosa] = useState(false);
  const [requierePago, setRequierePago] = useState(false);
  const [esErrorReserva, setEsErrorReserva] = useState(false);
  const [preferenceId, setPreferenceId] = useState(null);

  // Estado para error al eliminar turno
  const [errorEliminar, setErrorEliminar] = useState("");

  // Estado para abonos
  const [abonoInfo, setAbonoInfo] = useState(null);
  const [isAbonoModalOpen, setIsAbonoModalOpen] = useState(false);
  const [loadingAbono, setLoadingAbono] = useState(false);

  const navigate = useNavigate();
  const location = useLocation();

  // Revisar si volvimos de Mercado Pago con error (pagos estándar)
  useEffect(() => {
    const params = new URLSearchParams(location.search);
    if (params.get("pago") === "rechazado") {
      alert("El pago no pudo completarse. Se ha cancelado la reserva y se restauró el cupo de la clase.");
      // Limpiar URL
      navigate("/turnos", { replace: true });
    }

    const abonoStatus = params.get("abono");
    if (abonoStatus === "exitoso") {
      alert("Listo! Te abonaste exitosamente a la actividad.");
      navigate("/turnos", { replace: true });
    } else if (abonoStatus === "rechazado") {
      alert("El pago de tu abono no pudo completarse. Se canceló la operación.");
      navigate("/turnos", { replace: true });
    } else if (abonoStatus === "error_interno") {
      alert("Ocurrió un error al procesar el abono luego del pago.");
      navigate("/turnos", { replace: true });
    }
  }, [location, navigate]);

  const cargarTurnos = async () => {
    try {
      const url = viendoHistorial ? "http://localhost:5266/api/turnos/anteriores" : "http://localhost:5266/api/turnos";
      const response = await fetch(url);
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
  }, [viendoHistorial]);

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
    setInfoHistoricaTurno(null);

    if (viendoHistorial) {
      try {
        const res = await fetch(`http://localhost:5266/api/turnos/${turno.id}/info-historica`);
        if (res.ok) {
          const data = await res.json();
          setInfoHistoricaTurno(data);
        }
      } catch (e) {
        console.error("Error cargando info histórica del turno:", e);
      }
    }

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

  const handleEliminarTurno = async (idTurno) => {
    if (!window.confirm("¿Seguro que querés eliminar este turno?")) return;
    setErrorEliminar("");
    try {
      const response = await fetch(`http://localhost:5266/api/turnos/${idTurno}`, {
        method: 'DELETE',
      });
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || errorData.mensaje || "Error al eliminar el turno.");
      }
      alert("Turno eliminado con éxito");
      cerrarModal();
      cargarTurnos();
    } catch (err) {
      console.error(err);
      setErrorEliminar(err.message);
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
    setErrorEliminar("");
    setIsAbonoModalOpen(false);
    setAbonoInfo(null);
  };

  const handleInfoAbono = async () => {
    setLoadingAbono(true);
    try {
      const response = await fetch(`http://localhost:5266/api/abonos/info?idTurno=${modalTurno.id}&email=${encodeURIComponent(user.email)}`);
      const data = await response.json();

      if (response.ok) {
        setAbonoInfo(data);
        setIsAbonoModalOpen(true);
      } else {
        alert(data.message || "Error al obtener información del abono");
      }
    } catch (error) {
      console.error(error);
      alert("Error de conexión al obtener información del abono");
    } finally {
      setLoadingAbono(false);
    }
  };

  const handleReservar = async () => {
    if (!window.confirm("¿Estás seguro que querés reservar este turno?")) return;
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

  // HARDCODEO DE PAGOS: este bloque reemplaza el flujo de Mercado Pago por un pago local inmediato.
  const handlePagarReservaLocal = async () => {
    setLoadingReserva(true);
    setMensajeReserva("");
    setEsErrorReserva(false);

    try {
      const response = await fetch("http://localhost:5266/api/pagos/procesar-pago-local", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          idTurno: modalTurno.id,
          email: user.email
        })
      });

      const data = await response.json();

      if (response.ok) {
        setEsErrorReserva(false);
        setMensajeReserva(data.mensaje || "Pago registrado correctamente. Tu reserva quedó confirmada.");
        setReservaExitosa(true);
        setRequierePago(false);
        cargarTurnos();
      } else {
        setEsErrorReserva(true);
        setMensajeReserva(data.message || data.mensaje || "No se pudo procesar el pago local.");
      }
    } catch (error) {
      setEsErrorReserva(true);
      setMensajeReserva("Error de conexión con el servidor.");
    } finally {
      setLoadingReserva(false);
    }
  };

  const handleEntrarListaEspera = async () => {
    setLoadingReserva(true);
    setMensajeReserva("");
    setEsErrorReserva(false);

    try {
      const response = await fetch("http://localhost:5266/api/ListasDeEspera/entrar", {
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
      } else {
        setEsErrorReserva(true);
        setMensajeReserva(data.mensaje || "Ocurrió un error al intentar entrar a la lista de espera.");
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
        <h1>{viendoHistorial ? "Historial de Turnos" : "Gestión de Turnos"}</h1>
        {user?.esAdmin && (
          <div style={{ display: "flex", justifyContent: "center", gap: "10px", marginTop: "10px" }}>
            <button
              className={`btn ${!viendoHistorial ? 'btn-primary' : 'btn-secondary'}`}
              onClick={() => setViendoHistorial(false)}
            >
              Ver turnos actuales
            </button>
            <button
              className={`btn ${viendoHistorial ? 'btn-primary' : 'btn-secondary'}`}
              onClick={() => setViendoHistorial(true)}
            >
              Ver historial de turnos
            </button>
          </div>
        )}
      </div>

      {user?.esAdmin && !viendoHistorial && (
        <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '2rem' }}>
          <Link to="/turnos/crear">
            <button className="btn btn-primary">+ Crear Turno</button>
          </Link>
        </div>
      )}

      {turnos.length === 0 ? (
        <p>{viendoHistorial ? "No hay turnos anteriores cargados" : "Por el momento no hay turnos disponibles"}</p>
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
              {/* 
              <p style={{ color: "var(--text-muted)" }}>
                Click para ver detalles y reservar
              </p>
              */}
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
            
            {viendoHistorial && infoHistoricaTurno && (
              <>
                <p><strong>Inscriptos:</strong> {infoHistoricaTurno.inscriptos}</p>
                <p><strong>Asistencias:</strong> {infoHistoricaTurno.asistencias}</p>
              </>
            )}

            <hr style={{ margin: "15px 0" }} />

            {viendoHistorial ? null : loadingUser ? (
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
                {userInfo?.creditos > 0 && !reservaExitosa && (
                  <div className="alert alert-success" style={{ marginBottom: "15px" }}>
                    Tenés créditos disponibles para reservar
                  </div>
                )}
                {!reservaExitosa ? (
                  <>
                    {modalTurno.cupo > 0 ? (
                      <div style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
                        <button
                          onClick={handleReservar}
                          disabled={loadingReserva || loadingAbono}
                          className="btn btn-primary"
                          style={{ width: "100%" }}
                        >
                          {loadingReserva ? "Procesando..." : "Reservar turno"}
                        </button>
                        {modalTurno.idHorario && (
                          <button
                            onClick={handleInfoAbono}
                            disabled={loadingReserva || loadingAbono}
                            className="btn btn-secondary"
                            style={{ width: "100%", background: "var(--c-azul-medio)", color: "white" }}
                          >
                            {loadingAbono ? "Cargando..." : "Abonarse"}
                          </button>
                        )}
                      </div>
                    )
                      : (
                        <div style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
                          <button onClick={handleEntrarListaEspera} className="btn btn-secondary" style={{ width: "100%" }}>
                            Entrar a lista de espera
                          </button>
                          {modalTurno.idHorario && (
                            <button
                              onClick={handleInfoAbono}
                              disabled={loadingReserva || loadingAbono}
                              className="btn btn-secondary"
                              style={{ width: "100%", background: "var(--c-azul-medio)", color: "white" }}
                            >
                              {loadingAbono ? "Cargando..." : "Abonarse"}
                            </button>
                          )}
                        </div>
                      )
                      /* :   
                      (
                          <div className="alert alert-warning">
                            Por el momento no hay más cupos para esta actividad
                          </div>
                        )
                      */
                    }
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
                        {/* HARDCODEO DE PAGOS: este botón reemplaza al widget de Mercado Pago por un pago local directo. */}
                        <button
                          onClick={handlePagarReservaLocal}
                          disabled={loadingReserva}
                          className="btn btn-primary"
                          style={{ width: "100%" }}
                        >
                          {loadingReserva ? "Procesando..." : "Pagar ahora"}
                        </button>
                      </div>
                    )}

                    {/* HARDCODEO DE PAGOS: se conserva el bloque original de Mercado Pago comentado como referencia.
                    {preferenceId ? (
                      <Wallet initialization={{ preferenceId: preferenceId }} customization={{ texts: { action: 'pay' } }} />
                    ) : (
                      <p style={{ color: "var(--text-muted)" }}>Cargando botón de pago...</p>
                    )}
                    */}
                    <div style={{ marginTop: "20px" }}>
                      <Link to="/reservas" style={{ color: "var(--primary)", fontWeight: "bold" }}>Ir a Mis Reservas</Link>
                    </div>
                  </div>
                )}
              </div>
            )}

            {user?.esAdmin && !viendoHistorial && (
              <div style={{ marginTop: "15px", display: "flex", flexDirection: "column", gap: "10px" }}>
                <BotonModificarTurno onClick={() => modificarTurno(modalTurno.id)} />

                <BotonCancelarTurno idTurno={modalTurno.id} />

                <button
                  onClick={() => handleEliminarTurno(modalTurno.id)}
                  className="btn btn-danger"
                  style={{ width: "100%" }}
                >
                  Eliminar Turno
                </button>
                {errorEliminar && (
                  <div className="alert alert-error">
                    {errorEliminar}
                  </div>
                )}
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

      {isAbonoModalOpen && abonoInfo && (
        <AbonoInfoModal
          info={abonoInfo}
          turnoId={modalTurno?.id}
          userEmail={user?.email}
          onClose={() => setIsAbonoModalOpen(false)}
        />
      )}
    </div>
  );
}

export default TurnoPage;
