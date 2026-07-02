function RegistrarEmpleadoForm({
  nombreCompleto,
  setNombreCompleto,
  fechaNacimiento,
  setFechaNacimiento,
  dni,
  setDni,
  email,
  setEmail,
  password,
  setPassword,
  confirmPassword,
  setConfirmPassword,
  onSubmit,
  loading,
  error,
  success,
}) {
  return (
    <form onSubmit={onSubmit}>
      <div className="form-group">
        <label>Nombre Completo</label>
        <input
          type="text"
          placeholder="Don Juan de la Mancha"
          value={nombreCompleto}
          onChange={(e) => setNombreCompleto(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label>Fecha de nacimiento</label>
        <input
          type="date"
          value={fechaNacimiento}
          onChange={(e) => setFechaNacimiento(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label>DNI</label>
        <input
          type="text"
          placeholder="12345678"
          value={dni}
          onChange={(e) => setDni(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label>Email</label>
        <input
          type="email"
          placeholder="empleado@sportify.com"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label>Contraseña</label>
        <input
          type="password"
          placeholder="••••••••"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label>Confirmar contraseña</label>
        <input
          type="password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
        />
      </div>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      <button
        type="submit"
        disabled={loading}
        className="btn btn-primary"
        style={{ width: "100%", marginTop: "0.25rem" }}
      >
        {loading ? "Registrando..." : "Registrar empleado"}
      </button>
    </form>
  );
}

export default RegistrarEmpleadoForm;