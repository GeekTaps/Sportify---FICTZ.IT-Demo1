import { useState } from "react";

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
  success
}) {
  return (
    <form onSubmit={onSubmit}>
      

      <div>
        <label>Nombre Completo</label>
        <br />
        <input
          type="text"
          value={nombreCompleto}
          onChange={(e) => setNombreCompleto(e.target.value)}
        />
      </div>

      <div>
        <label>Edad</label>
        <br />
        <input
          type="text"
          value={edad}
          onChange={(e) => setEdad(e.target.value)}
        />
      </div>

      <div>
        <label>DNI</label>
        <br />
        <input
          type="text"
          value={dni}
          onChange={(e) => setDni(e.target.value)}
        />
      </div>

      <div>
        <label>Email</label>
        <br />
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>

      <div>
        <label>Contraseña</label>
        <br />
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>

      <br />

      <button type="submit" disabled={loading}>
        {loading ? "Modificando..." : "Modificar usuario"}
      </button>

      {error && <p style={{ color: "red" }}>{error}</p>}
      {success && <p style={{ color: "green" }}>{success}</p>}
    </form>
  );
}

export default ModificarUsuarioForm;