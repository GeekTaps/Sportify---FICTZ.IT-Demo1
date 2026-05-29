function CrearModificarTurnoForm({
  fecha,
  setFecha,
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
  horaFin,
  setHoraFin,
  deportes,
  onSubmit,
  loading,
  error,
  success,
  isModifying = false,
}) {
  return (
    <form onSubmit={onSubmit} style={{ maxWidth: "520px" }}>
      <div className="form-group">
        <label htmlFor="deporte">Deporte</label>
        <select
          id="deporte"
          value={idDeporte}
          onChange={(e) => setIdDeporte(e.target.value)}
          required
        >
          <option value="">Seleccioná un deporte</option>
          {deportes.map((deporte) => (
            <option key={deporte.id} value={deporte.id}>
              {deporte.nombre}
            </option>
          ))}
        </select>
      </div>

      <div className="form-group">
        <label htmlFor="fecha">Fecha</label>
        <input
          id="fecha"
          type="date"
          value={fecha}
          onChange={(e) => setFecha(e.target.value)}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="horaInicio">Hora de Inicio</label>
        <input
          id="horaInicio"
          type="time"
          value={horaInicio}
          onChange={(e) => setHoraInicio(e.target.value)}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="horaFin">Hora de Fin</label>
        <input
          id="horaFin"
          type="time"
          value={horaFin}
          onChange={(e) => setHoraFin(e.target.value)}
          required
        />
      </div>

      <div className="form-group">
        <label htmlFor="cupo">Cupo (cantidad de personas)</label>
        <input
          id="cupo"
          type="number"
          value={cupo}
          onChange={(e) => setCupo(e.target.value)}
          required
          min="1"
        />
      </div>

      <div className="form-group">
        <label htmlFor="profesor">Nombre del Profesor</label>
        <input
          id="profesor"
          type="text"
          value={nommbreProfesor}
          onChange={(e) => setNommbreProfesor(e.target.value)}
          required
          placeholder="Nombre del profesor a cargo"
        />
      </div>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      <button
        type="submit"
        disabled={loading}
        className="btn btn-primary"
        style={{ marginTop: "0.5rem" }}
      >
        {loading
          ? "Guardando..."
          : isModifying
          ? "Modificar Turno"
          : "Crear Turno"}
      </button>
    </form>
  );
}

export default CrearModificarTurnoForm;
