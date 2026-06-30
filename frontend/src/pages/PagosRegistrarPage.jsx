import { useEffect, useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { apiClient } from "../api/api-client";

function PagosRegistrarPage() {
  const { user } = useContext(AuthContext);
  const [usuarios, setUsuarios] = useState([]);
  const [filtroUsuario, setFiltroUsuario] = useState("");
  const [reservas, setReservas] = useState([]);
  const [selectedUsuarioId, setSelectedUsuarioId] = useState("");
  const [selectedReservaId, setSelectedReservaId] = useState("");
  const [mensaje, setMensaje] = useState("");
  const [mensajeTipo, setMensajeTipo] = useState("warning");
  const [cargandoUsuarios, setCargandoUsuarios] = useState(false);
  const [cargandoReservas, setCargandoReservas] = useState(false);
  const [registrando, setRegistrando] = useState(false);

  useEffect(() => {
    cargarUsuarios();
  }, []);

  const cargarUsuarios = async () => {
    try {
      setCargandoUsuarios(true);
      const response = await apiClient.get("/usuarios");
      const clientes = (response.data || []).filter(u => !u.esAdmin);
      setUsuarios(clientes);
    } catch (error) {
      console.error("Error al cargar usuarios:", error);
      setMensaje("No se pudieron cargar los usuarios. Intenta nuevamente.");
      setMensajeTipo("error");
    } finally {
      setCargandoUsuarios(false);
    }
  };

  const cargarReservas = async (usuarioId) => {
    if (!usuarioId) {
      setReservas([]);
      return;
    }

    try {
      setCargandoReservas(true);
      const response = await apiClient.get(`/Reservas/usuario/${usuarioId}`);

      const reservasFuturas = response.data || [];

      setReservas(reservasFuturas);
      setSelectedReservaId("");
      if (reservasFuturas.length === 0) {
        setMensaje("El usuario seleccionado no posee reservas");
        setMensajeTipo("warning");
      } else {
        setMensaje("");
      }
    } catch (error) {
      console.error("Error al cargar reservas:", error);
      const errorMessage = error.response?.data?.mensaje || error.response?.data?.message || "No se pudieron cargar las reservas.";
      setReservas([]);
      setSelectedReservaId("");
      setMensaje(errorMessage);
      setMensajeTipo("error");
    } finally {
      setCargandoReservas(false);
    }
  };

  const handleUsuarioChange = async (event) => {
    const usuarioId = event.target.value;
    setSelectedUsuarioId(usuarioId);
    setMensaje("");
    setMensajeTipo("warning");
    await cargarReservas(usuarioId);
  };

  const handleReservaChange = (event) => {
    setSelectedReservaId(event.target.value);
    setMensaje("");
    setMensajeTipo("warning");
  };

  const handleRegistrarPago = async () => {
    if (!selectedUsuarioId) {
      setMensaje("Debes seleccionar un usuario antes de registrar un pago.");
      setMensajeTipo("error");
      return;
    }
    if (!selectedReservaId) {
      setMensaje("Debes seleccionar una reserva para marcar como pagada.");
      setMensajeTipo("error");
      return;
    }

    try {
      setRegistrando(true);
      const response = await apiClient.post("/pagos/registrar", {
        idUsuario: selectedUsuarioId,
        idReserva: selectedReservaId,
      });

      setMensaje("Se registró el pago correctamente");
      setMensajeTipo("success");
      cargarReservas(selectedUsuarioId);
    } catch (error) {
      console.error("Error al registrar pago:", error);
      const errorMessage = error.response?.data?.mensaje || error.response?.data?.message || "Error al registrar el pago.";
      setMensaje(errorMessage);
      setMensajeTipo("error");
    } finally {
      setRegistrando(false);
    }
  };

  if (!user?.esAdmin) {
    return (
      <div className="page-header">
        <h1>Acceso denegado</h1>
        <p>Solo los administradores pueden registrar pagos presenciales.</p>
      </div>
    );
  }

  return (
    <div>
      <div className="page-header">
        <h1>Registrar Pagos Presenciales</h1>
        <p>Seleccioná un usuario y la reserva correspondiente para marcarla como pagada.</p>
      </div>

      {mensaje && (
        <div className={`alert ${mensajeTipo === "success" ? "alert-success" : mensajeTipo === "error" ? "alert-error" : "alert-warning"}`}>
          {mensaje}
        </div>
      )}

      <div className="form-card" style={{ maxWidth: 760, margin: "0 auto" }}>
        <div className="form-group">
          <label>Usuario</label>
          <input
            type="text"
            placeholder="Buscar por nombre o email..."
            value={filtroUsuario}
            onChange={(e) => setFiltroUsuario(e.target.value)}
            style={{ marginBottom: "0.5rem", padding: "0.5rem", width: "100%", borderRadius: "4px", border: "1px solid var(--border)" }}
          />
          <select value={selectedUsuarioId} onChange={handleUsuarioChange} disabled={cargandoUsuarios}>
            <option value="">-- Seleccioná un usuario --</option>
            {usuarios
              .filter(u =>
                (u.nombreCompleto?.toLowerCase() || "").includes(filtroUsuario.toLowerCase()) ||
                (u.email?.toLowerCase() || "").includes(filtroUsuario.toLowerCase())
              )
              .map((usuario) => (
                <option key={usuario.id} value={usuario.id}>
                  {usuario.email} · {usuario.nombreCompleto}
                </option>
              ))}
          </select>
        </div>

        <div className="form-group">
          <label>Reserva</label>
          <select
            value={selectedReservaId}
            onChange={handleReservaChange}
            disabled={!selectedUsuarioId || cargandoReservas || reservas.length === 0}
          >
            <option value="">-- Seleccioná una reserva --</option>
            {reservas.map((reserva) => (
              <option key={reserva.id} value={reserva.id}>
                {reserva.titulo} · ${reserva.monto} · {reserva.paga ? "Pagada" : "Pendiente"}
              </option>
            ))}
          </select>
        </div>

        <button
          className="btn btn-primary"
          onClick={handleRegistrarPago}
          disabled={registrando || !selectedUsuarioId || !selectedReservaId}
        >
          {registrando ? "Registrando pago..." : "Marcar como pagada"}
        </button>
      </div>


    </div>
  );
}

export default PagosRegistrarPage;
