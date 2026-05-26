function ModificarDeporteForm({ nombre, setNombre, descripcion, setDescripcion, onSubmit, loading, error, success }) {
  return (
    <form onSubmit={onSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "400px" }}>
      <label>
        Nombre del deporte:
        <input
          type="text"
          value={nombre}
          onChange={(event) => setNombre(event.target.value)}
          placeholder="Nuevo nombre"
        />
      </label>

      <label>
        Descripción del deporte:
        <textarea
          value={descripcion}
          onChange={(event) => setDescripcion(event.target.value)}
          placeholder="Nueva descripción"
          rows={4}
        />
      </label>

      {error && <div style={{ color: "red" }}>{error}</div>}
      {success && <div style={{ color: "green" }}>{success}</div>}

      <button type="submit" disabled={loading}>
        {loading ? "Modificando..." : "Modificar"}
      </button>
    </form>
  );
}

export default ModificarDeporteForm;
