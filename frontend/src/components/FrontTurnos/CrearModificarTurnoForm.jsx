function CrearModificarTurnoForm({
  diaSemana,
  setDiaSemana,
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
          Día de la semana:
          <select
            value={diaSemana}
            onChange={(e) => setDiaSemana(e.target.value)}
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
            <option value="">Selecciona un día</option>
            <option value="Lunes">Lunes</option>
            <option value="Martes">Martes</option>
            <option value="Miércoles">Miércoles</option>
            <option value="Jueves">Jueves</option>
            <option value="Viernes">Viernes</option>
            <option value="Sábado">Sábado</option>
            <option value="Domingo">Domingo</option>
          </select>
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
          Precio ($):
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

      <div style={{ marginBottom: "15px" }}>
        <label style={{ display: "flex", alignItems: "center", gap: "10px" }}>
          <input
            type="checkbox"
            checked={listaEsperaHabilitada}
            onChange={(e) => setListaEsperaHabilitada(e.target.checked)}
            style={{ transform: "scale(1.2)" }}
          />
          Habilitar Lista de Espera
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
