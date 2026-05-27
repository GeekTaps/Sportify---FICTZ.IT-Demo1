import { useState } from "react";

function ReservasPage() {
  const [reservas, setReservas] = useState([]);
  const [email, setEmail] = useState("");
  const [mensaje, setMensaje] = useState("");

  const cargarReservas = async () => {
    if (!email) {
      setMensaje("Por favor ingresa un email de usuario");
      return;
    }
    
    try {
      setMensaje("");
      const response = await fetch(`http://localhost:5266/api/Reservas/usuario/email/${email}`);
      
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

  return (
    <div>
      <h1>Mis Reservas</h1>
      <p>Visualiza tus reservas activas ingresando tu email.</p>

      <div style={{ marginBottom: "20px", display: "flex", justifyContent: "center", gap: "10px" }}>
        <input 
          type="email" 
          placeholder="Ingresa tu email (ej: milka123@mail.com)" 
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          style={{ padding: "8px", width: "300px" }}
        />
        <button onClick={cargarReservas} style={{ padding: "8px 16px", cursor: "pointer" }}>Buscar</button>
      </div>

      {mensaje && <p style={{ color: "red", fontWeight: "bold" }}>{mensaje}</p>}

      {reservas.length > 0 && (
        <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
          <ul style={{ listStyleType: "none", padding: 0, width: "100%", maxWidth: "600px" }}>
            {reservas.map((r) => (
              <li key={r.id} style={{ 
                marginBottom: "12px", 
                border: "1px solid #ccc", 
                padding: "15px", 
                borderRadius: "8px",
                textAlign: "left",
                backgroundColor: "#f9f9f9"
              }}>
                <h3 style={{ marginTop: 0, color: "#333" }}>{r.titulo}</h3>
                <strong>Estado de Pago:</strong> {r.paga ? "Pagada ✅" : "Pendiente de pago ❌"} <br/>
                <strong>Monto Total:</strong> ${r.monto}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}

export default ReservasPage;
