import React, { useState } from 'react';
// HARDCODEO DE PAGOS: se deja comentado el import original de Mercado Pago como referencia.
// import { Wallet } from '@mercadopago/sdk-react';

const AbonoInfoModal = ({ info, turnoId, userEmail, onClose }) => {
  const [loading, setLoading] = useState(false);
  const [mensaje, setMensaje] = useState('');
  const [esError, setEsError] = useState(false);
  const [preferenceId, setPreferenceId] = useState(null);

  const {
    isAlreadySubscribed,
    hasConflict,
    isPast10thDay,
    hasFewClasses,
    noCupo,
    actividad,
    horario,
    precioTotal,
    descuentoAplicado
  } = info;

  // Escenario 1: Ya abonado
  if (isAlreadySubscribed) {
    return (
      <div className="modal-overlay" onClick={onClose}>
        <div className="modal-content" onClick={(e) => e.stopPropagation()}>
          <h2 style={{ marginTop: 0, color: "var(--c-azul-cobalto)" }}>Abono a {actividad}</h2>
          <div className="alert alert-warning">Ya estás abonado a este turno.</div>
          <div style={{ display: "flex", justifyContent: "flex-end", marginTop: "20px" }}>
            <button onClick={onClose} className="btn btn-secondary">Cerrar</button>
          </div>
        </div>
      </div>
    );
  }

  // Escenario 2: Conflicto de horario
  if (hasConflict) {
    return (
      <div className="modal-overlay" onClick={onClose}>
        <div className="modal-content" onClick={(e) => e.stopPropagation()}>
          <h2 style={{ marginTop: 0, color: "var(--c-azul-cobalto)" }}>Abono a {actividad}</h2>
          <div className="alert alert-error">Ya tenés una reserva en ese horario.</div>
          <div style={{ display: "flex", justifyContent: "flex-end", marginTop: "20px" }}>
            <button onClick={onClose} className="btn btn-secondary">Cerrar</button>
          </div>
        </div>
      </div>
    );
  }

  // Escenario 3: Pasado el día 10
  if (isPast10thDay && hasFewClasses) {
    return (
      <div className="modal-overlay" onClick={onClose}>
        <div className="modal-content" onClick={(e) => e.stopPropagation()}>
          <h2 style={{ marginTop: 0, color: "var(--c-azul-cobalto)" }}>Abono a {actividad}</h2>
          <div className="alert alert-warning">
            Para abonarte este mes, debés hacerlo dentro de los primeros 10 días, o cuando queden al menos 3 clases en el mes.
          </div>
          <div style={{ display: "flex", justifyContent: "flex-end", marginTop: "20px" }}>
            <button onClick={onClose} className="btn btn-secondary">Cerrar</button>
          </div>
        </div>
      </div>
    );
  }

  // HARDCODEO DE PAGOS: este bloque reemplaza la creación de preferencia por un pago local inmediato.
  const handlePagarAbono = async () => {
    setLoading(true);
    setMensaje('');
    setEsError(false);
    
    try {
      const response = await fetch("http://localhost:5266/api/abonos/procesar-pago-local", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          idTurno: turnoId,
          email: userEmail
        })
      });

      if (response.ok) {
        const data = await response.json();
        setEsError(false);
        setMensaje(data.mensaje || "Pago registrado correctamente.");
      } else {
        const errData = await response.json();
        setEsError(true);
        setMensaje(errData.message || "Error al procesar el pago local.");
      }
    } catch (err) {
      setEsError(true);
      setMensaje("Error de red al conectar con el servidor.");
    } finally {
      setLoading(false);
    }
  };

  const handleEntrarListaEspera = async () => {
    setLoading(true);
    setMensaje('');
    setEsError(false);
    
    try {
      const response = await fetch("http://localhost:5266/api/listasdeespera/abono", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          idTurno: turnoId,
          email: userEmail
        })
      });

      const data = await response.json();
      
      if (response.ok) {
        setEsError(false);
        setMensaje(data.mensaje || "Te has anotado en la lista de espera exitosamente.");
      } else {
        setEsError(true);
        setMensaje(data.mensaje || "Ocurrió un error al intentar anotarte.");
      }
    } catch (err) {
      setEsError(true);
      setMensaje("Error de red al conectar con el servidor.");
    } finally {
      setLoading(false);
    }
  };

  // Escenario 4 y 6: Información de abono (con o sin cupo)
  // Escenario 5: Información con descuento
  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <h2 style={{ marginTop: 0, color: "var(--c-azul-cobalto)" }}>Abono a {actividad}</h2>
        <p><strong>Horario Fijo:</strong> {horario}</p>
        
        {descuentoAplicado > 0 && (
          <p style={{ color: "green", fontWeight: "bold" }}>
            ¡Tenés un descuento de ${descuentoAplicado} por tus créditos disponibles!
          </p>
        )}
        
        <p style={{ fontSize: "1.2rem", fontWeight: "bold" }}>
          <strong>Precio total a pagar:</strong> ${precioTotal}
        </p>

        {noCupo && (
           <div className="alert alert-warning" style={{ margin: "15px 0" }}>
             Por el momento no hay más cupos para abonarse a esta actividad.
           </div>
        )}

        {mensaje && (
          <div className={`alert ${esError ? 'alert-error' : 'alert-success'}`} style={{ marginTop: "15px", marginBottom: "15px" }}>
            {mensaje}
          </div>
        )}

        <div style={{ display: "flex", flexDirection: "column", gap: "10px", marginTop: "20px" }}>
          {!noCupo ? (
            /* HARDCODEO DE PAGOS: este botón reemplaza al widget de Mercado Pago por un pago local directo. */
            <button 
              className="btn btn-primary" 
              onClick={handlePagarAbono}
              disabled={loading}
            >
              {loading ? "Procesando..." : "Confirmar y Pagar"}
            </button>
          ) : (
            <button 
              className="btn btn-secondary" 
              onClick={handleEntrarListaEspera}
              disabled={loading || mensaje}
            >
              {loading ? "Procesando..." : "Entrar a lista de espera"}
            </button>
          )}
          
          {/* HARDCODEO DE PAGOS: se conserva el bloque original de Mercado Pago comentado como referencia.
          {preferenceId ? (
            <Wallet initialization={{ preferenceId }} customization={{ texts: { action: 'pay' } }} />
          ) : (
            <button 
              className="btn btn-primary" 
              onClick={handlePagarAbono}
              disabled={loading}
            >
              {loading ? "Procesando..." : "Confirmar y Pagar"}
            </button>
          )}
          */}
          <button onClick={onClose} className="btn" style={{ background: "var(--border)", color: "var(--text-main)" }}>
            Cancelar
          </button>
        </div>
      </div>
    </div>
  );
};

export default AbonoInfoModal;
