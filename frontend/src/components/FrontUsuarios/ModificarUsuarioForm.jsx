function ModificarUsuarioForm({
  nombreCompleto,
  setNombreCompleto,
  fechaNacimiento,
  setFechaNacimiento,
  dni,
  setDni,
  email,
  setEmail,

  passwordActual,
  setPasswordActual,

  passwordNueva,
  setPasswordNueva,

  confirmarPassword,
  setConfirmarPassword,

  onSubmit,
  loading,
  error,
  success,
})  {
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
  <label htmlFor="mod-fechaNacimiento">
    Fecha de nacimiento
  </label>

  <input
    id="mod-fechaNacimiento"
    type="date"
    value={fechaNacimiento}
    onChange={(e) =>
      setFechaNacimiento(e.target.value)
    }
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
        <label htmlFor="mod-passwordActual">Contraseña actual</label>
        <input
          id="mod-passwordActual"
          type="password"
          value={passwordActual}
          onChange={(e) => setPasswordActual(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="mod-passwordNueva">Nueva contraseña</label>
        <input
          id="mod-passwordNueva"
          type="password"
          value={passwordNueva}
          onChange={(e) => setPasswordNueva(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="mod-confirmarPassword">Confirmar nueva contraseña</label>
        <input
          id="mod-confirmarPassword"
          type="password"
          value={confirmarPassword}
          onChange={(e) => setConfirmarPassword(e.target.value)}
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
