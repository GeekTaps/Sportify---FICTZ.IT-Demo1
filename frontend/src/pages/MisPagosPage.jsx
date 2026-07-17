import { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";
import { apiClient } from "../api/api-client";
import { useNavigate } from "react-router-dom";
import mpLogo from "../assets/logo-mercadopago-sin-letras.png";

function MisPagosPage() {
  const { user } = useContext(AuthContext);
  const navigate = useNavigate();
  const [pagos, setPagos] = useState([]);
  const [pagosPendientes, setPagosPendientes] = useState([]);
  const [mensaje, setMensaje] = useState("");
  const [tipoMensaje, setTipoMensaje] = useState("warning");
  const [cargando, setCargando] = useState(false);
  const [consultado, setConsultado] = useState(false);
  const [vista, setVista] = useState("realizados"); // 'realizados' | 'pendientes'

  const handleListarPagos = async () => {
    if (!user?.id) return;
    setMensaje("");
    setTipoMensaje("warning");
    setCargando(true);
    setConsultado(true);
    try {
      const response = await apiClient.get(`/pagos/usuario/${user.id}`);
      const lista = response.data || [];
      setPagos(lista);
      if (lista.length === 0) {
        setMensaje("No hay pagos registrados");
        setTipoMensaje("warning");
      }
    } catch (error) {
      console.error("Error al listar pagos:", error);
      const errorMessage = error.response?.data?.mensaje || error.response?.data?.message || "Error al obtener los pagos.";
      setPagos([]);
      setMensaje(errorMessage);
      setTipoMensaje("error");
    } finally {
      setCargando(false);
    }
  };

  const handleListarPendientes = async () => {
    if (!user?.id) return;
    setMensaje("");
    setTipoMensaje("warning");
    setCargando(true);
    try {
      const response = await fetch(`http://localhost:5266/api/Reservas/usuario/activas/${user.id}`);
      if (response.ok) {
        const reservas = await response.json();
        // Filtrar reservas que requieren pago
        const pendientes = reservas.filter(r => (r.abonado && !r.paga) || (r.pagoSeña && !r.paga));
        setPagosPendientes(pendientes);
        if (pendientes.length === 0) {
          setMensaje("No hay pagos pendientes");
          setTipoMensaje("warning");
        }
      } else {
        const errData = await response.json();
        setPagosPendientes([]);
        setMensaje(errData.mensaje || "Error al obtener reservas activas.");
        setTipoMensaje("error");
      }
    } catch (error) {
      console.error("Error al listar pagos pendientes:", error);
      setPagosPendientes([]);
      setMensaje("Error de conexión al obtener los pagos pendientes.");
      setTipoMensaje("error");
    } finally {
      setCargando(false);
    }
  };

  useEffect(() => {
    if (user) {
      if (vista === "realizados") {
        handleListarPagos();
      } else if (vista === "pendientes") {
        handleListarPendientes();
      }
    }
  }, [user, vista]);

  if (!user) {
    return (
      <div className="page-header">
        <h1>Mis Pagos</h1>
        <p>Debes iniciar sesión para ver tus pagos.</p>
      </div>
    );
  }

  return (
    <div>
      <div className="page-header" style={{ marginBottom: "1rem" }}>
        <h1>Mis Pagos</h1>
        <p>Revisá los pagos realizados o pagá tus pendientes.</p>
      </div>

      <div style={{ display: "flex", justifyContent: "center", gap: "1rem", marginBottom: "2rem" }}>
        <button 
          className={vista === "realizados" ? "btn btn-primary" : "btn btn-secondary"}
          onClick={() => setVista("realizados")}
        >
          {vista === "pendientes" ? "Volver a Mis Pagos" : "Pagos realizados"}
        </button>
        <button 
          className={vista === "pendientes" ? "btn btn-primary" : "btn btn-secondary"}
          onClick={() => setVista("pendientes")}
        >
          Pagos pendientes
        </button>
      </div>

      {cargando && <p style={{ textAlign: "center" }}>Cargando...</p>}

      {!cargando && mensaje && (
        <div className={`alert ${tipoMensaje === "success" ? "alert-success" : tipoMensaje === "error" ? "alert-error" : "alert-warning"}`}>
          {mensaje}
        </div>
      )}

      {!cargando && vista === "realizados" && consultado && pagos.length > 0 && (
        <div className="card" style={{ maxWidth: 760, margin: "0 auto" }}>
          <ul className="reserva-list">
            {pagos.map((p) => (
              <li key={p.id} className="reserva-card">
                <div className="reserva-card-info">
                  <h3>{p.monto < 0 ? "Devolución" : "Pago"} #{p.id.substring(0, 8)}</h3>
                  <p>
                    <strong>Reserva:</strong> {p.tituloReserva || "Sin título"}
                  </p>
                  <p>
                    <strong>Monto:</strong> <span style={{ color: p.monto < 0 ? "var(--success)" : "inherit", fontWeight: p.monto < 0 ? "bold" : "normal" }}>
                      {p.monto < 0 ? `-$${Math.abs(p.monto)}` : `$${p.monto}`}
                    </span>
                  </p>
                  <p>
                    <strong>Fecha:</strong> {new Date(p.fecha).toLocaleString()}
                  </p>
                </div>
              </li>
            ))}
          </ul>
        </div>
      )}

      {!cargando && vista === "pendientes" && pagosPendientes.length > 0 && (
        <div className="card" style={{ maxWidth: 760, margin: "0 auto" }}>
          <ul className="reserva-list">
            {pagosPendientes.map((r) => (
              <li key={r.id} className="reserva-card">
                <div className="reserva-card-info">
                  <h3>{r.titulo || "Sin título"}</h3>
                  <p><strong>Tipo:</strong> {r.abonado ? "Cuota de abono mensual" : "Confirmación de seña"}</p>
                  <p><strong>Monto base:</strong> ${r.monto}</p>
                </div>
                <div className="reserva-card-actions">
                  <button
                    className="btn btn-primary"
                    style={{ backgroundColor: "#009ee3", color: "white", border: "none", display: "flex", alignItems: "center", justifyContent: "center", gap: "0.5rem" }}
                    onClick={async () => {
                      if (r.abonado) {
                        try {
                          const res = await fetch(`http://localhost:5266/api/abonos/info?idTurno=${r.idTurno}&email=${encodeURIComponent(user.email)}`);
                          const data = await res.json();
                          if (res.ok) {
                            navigate(`/pagar/mercado-pago?tipo=cuota_abono&idTurno=${r.idTurno}&email=${encodeURIComponent(user.email)}&monto=${data.precioTotal}`);
                          } else {
                            alert(data.message || "Error al obtener la información del abono.");
                          }
                        } catch (err) {
                          alert("Error de conexión al obtener el precio del abono.");
                        }
                      } else {
                        const montoAPagar = r.monto / 2;
                        navigate(`/pagar/mercado-pago?tipo=confirmacion&idTurno=${r.id}&email=${encodeURIComponent(user.email)}&monto=${montoAPagar}`);
                      }
                    }}
                  >
                    <img src={mpLogo} alt="Mercado Pago" style={{ width: '24px', height: '24px', objectFit: 'contain' }} />
                    Pagar con Mercado Pago
                  </button>
                </div>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}

export default MisPagosPage;
