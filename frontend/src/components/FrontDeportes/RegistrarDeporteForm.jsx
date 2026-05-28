import BotonRegistrarDeporte from "./BotonRegistrarDeporte";

function RegistrarDeporteForm({ nombre, setNombre, descripcion, setDescripcion, onSubmit, loading, error, success }) {
  return (
    <form onSubmit={onSubmit} style={{ display: "flex", flexDirection: "column", gap: "10px", maxWidth: "400px", marginBottom: "20px" }}>
      <label>
        Nombre del deporte:
        <input
          type="text"
          value={nombre}
          onChange={(event) => setNombre(event.target.value)}
          placeholder="Nombre del deporte"
        />
      </label>

      <label>
        Descripción del deporte:
        <textarea
          value={descripcion}
          onChange={(event) => setDescripcion(event.target.value)}
          placeholder="Descripción del deporte"
          rows={4}
        />
      </label>

      {error && <div style={{ color: "red" }}>{error}</div>}
      {success && <div style={{ color: "green" }}>{success}</div>}

      <BotonRegistrarDeporte loading={loading} />
    </form>
  );
}

export default RegistrarDeporteForm;
