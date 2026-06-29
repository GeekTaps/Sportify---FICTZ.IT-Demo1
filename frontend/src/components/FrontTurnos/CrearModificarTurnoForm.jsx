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
  inscriptosCount = 0,
}) {
  const minFecha = (() => {
    const hoy = new Date();
    return hoy.toISOString().split("T")[0];
  })();

  const minHora = (() => {
    if (fechaInicio === minFecha) {
      const hoy = new Date();
      return `${String(hoy.getHours()).padStart(2, "0")}:${String(hoy.getMinutes()).padStart(2, "0")}`;
    }
    return undefined;
  })();

  return (
    <form onSubmit={onSubmit} style={{ maxWidth: "520px" }} noValidate>
      <div className="form-group">
        <label htmlFor="deporte">Deporte</label>
        <select
          id="deporte"
          value={idDeporte}
          onChange={(e) => setIdDeporte(e.target.value)}
          required
          disabled={isModifying}
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
          disabled={isModifying}
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
          min={minHora}
          disabled={isModifying}
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
          min={isModifying ? inscriptosCount : 1}
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
          type="text"
          value={precio === "" ? "" : precio}
          readOnly
          placeholder="0"
        />
        <small style={{ display: "block", marginTop: "0.25rem", color: "#666" }}>
          {isModifying
            ? "El precio del turno se conserva y no se puede modificar."
            : "El precio se toma automáticamente del deporte seleccionado."}
        </small>
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
          ? "Guardar Cambios"
          : "Crear Turno"}
      </button>
    </form>
  );
}

export default CrearModificarTurnoForm;
