function ModificarUsuarioForm({
  nombreCompleto,
  setNombreCompleto,
  edad,
  setEdad,
  dni,
  setDni,
  email,
  setEmail,
  password,
  setPassword,
  onSubmit,
  loading,
  error,
  success,
}) {
  return (
    <form onSubmit={onSubmit} style={{ maxWidth: "440px" }}>
      <div className="form-group">
        <label htmlFor="mod-nombre">Nombre Completo</label>
        <input
          id="mod-nombre"
          type="text"
          value={nombreCompleto}
          onChange={(e) => setNombreCompleto(e.target.value)}
          placeholder="Nuevo nombre"
        />
      </div>

      <div className="form-group">
        <label htmlFor="mod-edad">Edad</label>
        <input
          id="mod-edad"
          type="number"
          value={edad}
          onChange={(e) => setEdad(e.target.value)}
          placeholder="Nueva edad"
          min="1"
        />
      </div>

      <div className="form-group">
        <label htmlFor="mod-dni">DNI</label>
        <input
          id="mod-dni"
          type="text"
          value={dni}
          onChange={(e) => setDni(e.target.value)}
          placeholder="Nuevo DNI"
        />
      </div>

      <div className="form-group">
        <label htmlFor="mod-email">Email</label>
        <input
          id="mod-email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Nuevo email"
        />
      </div>

      <div className="form-group">
        <label htmlFor="mod-password">Contraseña</label>
        <input
          id="mod-password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Nueva contraseña"
        />
      </div>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      <button type="submit" disabled={loading} className="btn btn-primary" style={{ marginTop: "0.5rem" }}>
        {loading ? "Modificando..." : "Guardar cambios"}
      </button>
    </form>
  );
}

export default ModificarUsuarioForm;
