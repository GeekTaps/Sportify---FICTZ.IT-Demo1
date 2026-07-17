import React, { useEffect, useState } from 'react';
import ItemAsistencia from '../components/ItemsAsistencia';

const ListarAsistenciasDeUsuario = () => {
  const [usuarios, setUsuarios] = useState([]);
  const [usuarioSeleccionado, setUsuarioSeleccionado] = useState('');
  const [asistencias, setAsistencias] = useState([]);
  const [cargandoUsuarios, setCargandoUsuarios] = useState(true);
  const [cargandoAsistencias, setCargandoAsistencias] = useState(false);

  // 1. Cargar la lista de usuarios al montar el componente
  useEffect(() => {
    const obtenerUsuarios = async () => {
      try {
        const response = await fetch('http://localhost:5266/api/usuarios/traer-clientes');
        
        if (!response.ok) {
          throw new Error(`Error en el servidor: ${response.status}`);
        }

        const data = await response.json();

        // Aplicamos el filtro aquí mismo para asegurar que solo queden clientes reales
        const clientesFiltrados = Array.isArray(data) 
          ? data.filter(u => !u.esAdmin && !u.esEmpleado && !u.borrado) 
          : [];

        setUsuarios(clientesFiltrados);
      } catch (error) {
        console.error("Error al cargar usuarios:", error);
        setUsuarios([]);
      } finally {
        setCargandoUsuarios(false);
      }
    };
    obtenerUsuarios();
  }, []);

  // 2. Cargar las asistencias del usuario seleccionado
  useEffect(() => {
    const obtenerAsistenciasDeUsuario = async () => {
      if (!usuarioSeleccionado) {
        setAsistencias([]);
        return;
      }
      setCargandoAsistencias(true);
      try {
        const response = await fetch(`/api/asistencias/usuario/${usuarioSeleccionado}`);
        if (!response.ok) {
          throw new Error(`Error al buscar asistencias: ${response.status}`);
        }
        const data = await response.json();
        setAsistencias(Array.isArray(data) ? data : []);
      } catch (error) {
        console.error("Error al cargar asistencias:", error);
        setAsistencias([]);
      } finally {
        setCargandoAsistencias(false);
      }
    };

    obtenerAsistenciasDeUsuario();
  }, [usuarioSeleccionado]);

  return (
    <div style={{ maxWidth: '700px', margin: '2rem auto', padding: '1.5rem', fontFamily: 'sans-serif' }}>
      <h2 style={{ color: '#0d47a1', marginBottom: '1.5rem', textAlign: 'center' }}>Control de Asistencias de Alumnos</h2>

      <div style={{ marginBottom: '2rem', textAlign: 'center' }}>
        <label htmlFor="user-select" style={{ display: 'block', fontSize: '1.1rem', marginBottom: '8px', fontWeight: 'bold' }}>
          Seleccionar un Alumno:
        </label>
        {cargandoUsuarios ? (
          <p>Cargando lista de alumnos...</p>
        ) : (
          <select 
            id="user-select"
            value={usuarioSeleccionado}
            onChange={(e) => setUsuarioSeleccionado(e.target.value)}
            style={{ padding: '12px', fontSize: '1rem', width: '100%', maxWidth: '400px', borderRadius: '8px', border: '1px solid #ccc' }}
          >
            <option value="">-- Seleccione un usuario --</option>
            {usuarios.map(u => (
              <option key={u.id} value={u.id}>
                {u.nombreCompleto} ({u.dni})
              </option>
            ))}
          </select>
        )}
      </div>

      <hr style={{ border: 'none', borderTop: '1px solid #eee', margin: '2rem 0' }} />

      {usuarioSeleccionado && (
        <div>
          {cargandoAsistencias ? (
            <p style={{ textAlign: 'center' }}>Consultando historial...</p>
          ) : asistencias.length === 0 ? (
            <p style={{ textAlign: 'center', color: '#666' }}>Este alumno no registra asistencias.</p>
          ) : (
            <ul style={{ listStyle: 'none', padding: 0 }}>
              {asistencias.map((asistencia) => (
                <ItemAsistencia 
                  key={asistencia.id}
                  nombreTurno={asistencia.turno?.nombre}
                  fecha={asistencia.turno?.fecha}
                  horario={asistencia.turno?.horario}
                  asiste={asistencia.asiste}
                />
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
};

export default ListarAsistenciasDeUsuario;