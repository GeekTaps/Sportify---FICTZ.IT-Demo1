function RegistrarDeporteForm({ nombre, setNombre, descripcion, setDescripcion, onSubmit, loading, error, success }) {
  return (
    <form onSubmit={onSubmit} style={{ maxWidth: "520px" }}>
      <div className="form-group">
        <label htmlFor="nombre-deporte">Nombre del deporte</label>
        <input
          id="nombre-deporte"
          type="text"
          value={nombre}
          onChange={(event) => setNombre(event.target.value)}
          placeholder="Ej: Natación"
        />
      </div>

      <div className="form-group">
        <label htmlFor="descripcion-deporte">Descripción del deporte</label>
        <textarea
          id="descripcion-deporte"
          value={descripcion}
          onChange={(event) => setDescripcion(event.target.value)}
          placeholder="Breve descripción de la actividad"
          rows={4}
        />
      </div>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      <button type="submit" disabled={loading} className="btn btn-primary" style={{ marginTop: "0.5rem" }}>
        {loading ? "Registrando..." : "Registrar deporte"}
      </button>
    </form>
  );
}

export default RegistrarDeporteForm;
