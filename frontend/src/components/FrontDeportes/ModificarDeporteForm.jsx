function ModificarDeporteForm({
  nombre,
  setNombre,
  descripcion,
  setDescripcion,
  onSubmit,
  loading,
  error,
  success,
}) {
  return (
    <form onSubmit={onSubmit} style={{ maxWidth: "520px" }}>
      <div className="form-group">
        <label htmlFor="nombre-deporte">Nombre del deporte (no modificable)</label>
        <input
          id="nombre-deporte"
          type="text"
          value={nombre}
          readOnly
          placeholder="Nombre del deporte"
        />
      </div>

      <div className="form-group">
        <label htmlFor="descripcion-deporte">Descripción del deporte</label>
        <textarea
          id="descripcion-deporte"
          value={descripcion}
          onChange={(e) => setDescripcion(e.target.value)}
          placeholder="Nueva descripción"
          rows={4}
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
        {loading ? "Modificando..." : "Modificar"}
      </button>
    </form>
  );
}

export default ModificarDeporteForm;
