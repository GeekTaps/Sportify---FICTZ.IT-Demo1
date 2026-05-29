import { useState } from "react";

function CrearDeportePage() {

  const [deportes, setDeportes] = useState([]);
  const [nombre, setNombre] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const cargarDeportes = async () => {

    try {

      const response = await fetch("http://localhost:5266/api/deportes");

      const data = await response.json();

      setDeportes(data);

    } catch (error) {

      console.error(error);

    }
  };

  const registrarDeporte = async (event) => {

    event.preventDefault();

    setError("");
    setSuccess("");

    if (!nombre.trim() || !descripcion.trim()) {

      setError("Completa todos los campos");

      return;
    }

    try {

      const response = await fetch(
        "http://localhost:5266/api/deportes",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            nombre,
            descripcion,
          }),
        }
      );

      if (response.ok) {

        setSuccess("Deporte registrado correctamente");

        setNombre("");
        setDescripcion("");

        cargarDeportes();

      } else {

        setError("Error al registrar deporte");

      }

    } catch (error) {

      console.error(error);

      setError("Error al registrar deporte");

    }
  };

  return (

    <div>

      <h1>Registrar deporte</h1>

      <form onSubmit={registrarDeporte}>

        <input
          type="text"
          placeholder="Nombre"
          value={nombre}
          onChange={(e) => setNombre(e.target.value)}
        />

        <br />

        <textarea
          placeholder="Descripción"
          value={descripcion}
          onChange={(e) => setDescripcion(e.target.value)}
        />

        <br />

        <button type="submit">
          Registrar deporte
        </button>

      </form>

      <br />

      <button onClick={cargarDeportes}>
        Mostrar deportes
      </button>

      {error && <p>{error}</p>}

      {success && <p>{success}</p>}

      <ul>

        {deportes.map((d) => (

          <li key={d.id}>

            <strong>{d.nombre}</strong> - {d.descripcion}

          </li>

        ))}

      </ul>

    </div>
  );
}

export default CrearDeportePage;