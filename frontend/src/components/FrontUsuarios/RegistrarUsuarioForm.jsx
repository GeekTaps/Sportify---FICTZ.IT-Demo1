function RegisterForm({
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
    <form onSubmit={onSubmit}>
      <div className="form-group">
        <label htmlFor="reg-nombre">Nombre Completo</label>
        <input
          id="reg-nombre"
          type="text"
          placeholder="Juan Pérez"
          value={nombreCompleto}
          onChange={(e) => setNombreCompleto(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="reg-edad">Edad</label>
        <input
          id="reg-edad"
          type="number"
          placeholder="25"
          value={edad}
          onChange={(e) => setEdad(e.target.value)}
          min="1"
        />
      </div>

      <div className="form-group">
        <label htmlFor="reg-dni">DNI</label>
        <input
          id="reg-dni"
          type="text"
          placeholder="12345678"
          value={dni}
          onChange={(e) => setDni(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="reg-email">Email</label>
        <input
          id="reg-email"
          type="email"
          placeholder="tu@email.com"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>

      <div className="form-group">
        <label htmlFor="reg-password">Contraseña</label>
        <input
          id="reg-password"
          type="password"
          placeholder="••••••••"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
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
        {loading ? "Registrando..." : "Registrarse"}
      </button>
    </form>
  );
}

export default RegisterForm;
