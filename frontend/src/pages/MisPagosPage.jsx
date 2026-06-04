import { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { apiClient } from "../api/api-client";

function MisPagosPage() {
  const { user } = useContext(AuthContext);
  const [pagos, setPagos] = useState([]);
  const [mensaje, setMensaje] = useState("");
  const [tipoMensaje, setTipoMensaje] = useState("warning");
  const [cargando, setCargando] = useState(false);
  const [consultado, setConsultado] = useState(false);

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
      <div className="page-header">
        <h1>Mis Pagos</h1>
        <p>Revisá los pagos que realizaste anteriormente.</p>
      </div>

      <div className="form-card" style={{ maxWidth: 760, margin: "0 auto 1.5rem" }}>
        <button
          className="btn btn-primary"
          onClick={handleListarPagos}
          disabled={cargando}
          style={{ minWidth: 180 }}
        >
          {cargando ? "Listando pagos..." : "Listar mis pagos"}
        </button>
      </div>

      {mensaje && (
        <div className={`alert ${tipoMensaje === "success" ? "alert-success" : tipoMensaje === "error" ? "alert-error" : "alert-warning"}`}>
          {mensaje}
        </div>
      )}

      {consultado && pagos.length > 0 && (
        <div className="card" style={{ maxWidth: 760, margin: "0 auto" }}>
          <ul className="reserva-list">
            {pagos.map((p) => (
              <li key={p.id} className="reserva-card">
                <div className="reserva-card-info">
                  <h3>Pago #{p.id.substring(0, 8)}</h3>
                  <p>
                    <strong>Reserva:</strong> {p.idReserva}
                  </p>
                  <p>
                    <strong>Monto:</strong> ${p.monto}
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
    </div>
  );
}

export default MisPagosPage;
