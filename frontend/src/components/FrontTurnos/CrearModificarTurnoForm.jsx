function CrearModificarTurnoForm({
  fechaInicio,
  setFechaInicio,
  cupo,
  setCupo,
  idDeporte,
  setIdDeporte,
  nombreTurno,
  setNombreTurno,
  nommbreProfesor,
  setNommbreProfesor,
  horaInicio,
  setHoraInicio,
  precio,
  setPrecio,
  listaEsperaHabilitada,
  setListaEsperaHabilitada,
  deportes,
  onSubmit,
  loading,
  error,
  success,
  isModifying = false,
}) {
  const minFecha = (() => {
    const hoy = new Date();
    hoy.setDate(hoy.getDate() + 1);
    return hoy.toISOString().split("T")[0];
  })();

  return (
    <form onSubmit={onSubmit}>
      <div style={{ marginBottom: "15px" }}>
        <label>
          Deporte:
          <select
            value={idDeporte}
            onChange={(e) => setIdDeporte(e.target.value)}
            required
            style={{
              display: "block",
              marginTop: "5px",
              padding: "8px",
              width: "100%",
              borderRadius: "4px",
              border: "1px solid #ddd",
            }}
          >
            <option value="">Selecciona un deporte</option>
            {deportes.map((deporte) => (
              <option key={deporte.id} value={deporte.id}>
                {deporte.nombre}
              </option>
            ))}
          </select>
        </label>
      </div>

      <div style={{ marginBottom: "15px" }}>
        <label>
          Fecha de inicio:
          <input
            type="date"
            value={fechaInicio}
            onChange={(e) => setFechaInicio(e.target.value)}
            required
            min={minFecha}
            style={{
              display: "block",
              marginTop: "5px",
              padding: "8px",
              width: "100%",
              borderRadius: "4px",
              border: "1px solid #ddd",
            }}
          />
        </label>
      </div>

      <div style={{ marginBottom: "15px" }}>
        <label>
          Hora de Inicio:
          <input
            type="time"
            value={horaInicio}
            onChange={(e) => setHoraInicio(e.target.value)}
            required
            style={{
              display: "block",
              marginTop: "5px",
              padding: "8px",
              width: "100%",
              borderRadius: "4px",
              border: "1px solid #ddd",
            }}
          />
        </label>
      </div>



      <div style={{ marginBottom: "15px" }}>
        <label>
          Cupo (cantidad de personas):
          <input
            type="number"
            value={cupo}
            onChange={(e) => setCupo(e.target.value)}
            required
            min="1"
            style={{
              display: "block",
              marginTop: "5px",
              padding: "8px",
              width: "100%",
              borderRadius: "4px",
              border: "1px solid #ddd",
            }}
          />
        </label>
      </div>

      <div style={{ marginBottom: "15px" }}>
        <label>
          Nombre del Profesor:
          <input
            type="text"
            value={nommbreProfesor}
            onChange={(e) => setNommbreProfesor(e.target.value)}
            required
            style={{
              display: "block",
              marginTop: "5px",
              padding: "8px",
              width: "100%",
              borderRadius: "4px",
              border: "1px solid #ddd",
            }}
          />
        </label>
      </div>

      <div style={{ marginBottom: "15px" }}>
        <label>
          Precio:
          <input
            type="number"
            min="0"
            step="100"
            value={precio}
            onChange={(e) => setPrecio(e.target.value)}
            required
            style={{
              display: "block",
              marginTop: "5px",
              padding: "8px",
              width: "100%",
              borderRadius: "4px",
              border: "1px solid #ddd",
            }}
          />
        </label>
      </div>

      

      {error && (
        <div
          style={{
            color: "red",
            marginBottom: "15px",
            padding: "10px",
            backgroundColor: "#ffebee",
            borderRadius: "4px",
          }}
        >
          {error}
        </div>
      )}

      {success && (
        <div
          style={{
            color: "green",
            marginBottom: "15px",
            padding: "10px",
            backgroundColor: "#e8f5e9",
            borderRadius: "4px",
          }}
        >
          {success}
        </div>
      )}

      <button
        type="submit"
        disabled={loading}
        style={{
          padding: "10px 20px",
          fontSize: "16px",
          backgroundColor: loading ? "#cccccc" : "#4CAF50",
          color: "white",
          border: "none",
          borderRadius: "4px",
          cursor: loading ? "not-allowed" : "pointer",
        }}
      >
        {loading ? "Guardando..." : isModifying ? "Modificar Turno" : "Crear Turno"}
      </button>
    </form>
  );
}

export default CrearModificarTurnoForm;
