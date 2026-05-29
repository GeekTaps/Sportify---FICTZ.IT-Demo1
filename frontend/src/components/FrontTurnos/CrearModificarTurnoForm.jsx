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
        <label htmlFor="fechaInicio">Fecha de inicio</label>
        <input
          id="fechaInicio"
          type="date"
          value={fechaInicio}
          onChange={(e) => setFechaInicio(e.target.value)}
          required
          min={minFecha}
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

      <div className="form-group">
        <label htmlFor="precio">Precio</label>
        <input
          id="precio"
          type="number"
          min="0"
          step="100"
          value={precio}
          onChange={(e) => setPrecio(e.target.value)}
          required
          placeholder="0"
        />
      </div>

      {/* 
      <div className="form-group" style={{ flexDirection: "row", alignItems: "center", gap: "0.6rem" }}>
        <input
          id="listaEspera"
          type="checkbox"
          checked={listaEsperaHabilitada}
          onChange={(e) => setListaEsperaHabilitada(e.target.checked)}
          style={{ width: "auto", padding: 0 }}
        />
        <label htmlFor="listaEspera" style={{ margin: 0 }}>
          Habilitar lista de espera cuando se agote el cupo
        </label>
      </div>
      */}

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
