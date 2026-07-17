import React, { useEffect, useState } from 'react';
import ItemAsistencia from '../components/ItemsAsistencia'; // 🌟 IMPORTADO DESDE COMPONENTS

const ListarAsistenciasDeUsuario = () => {
  const [usuarios, setUsuarios] = useState([]);
  const [usuarioSeleccionado, setUsuarioSeleccionado] = useState('');
  const [asistencias, setAsistencias] = useState([]);
  const [cargandoUsuarios, setCargandoUsuarios] = useState(true);
  const [cargandoAsistencias, setCargandoAsistencias] = useState(false);

  // 1. Cargar la lista de todos los usuarios registrados al montar el componente
  useEffect(() => {
    const obtenerUsuarios = async () => {
      try {
        const response = await fetch('/api/usuarios');
        const data = await response.json();
        setUsuarios(data);
      } catch (error) {
        console.error("Error al cargar usuarios:", error);
      } finally {
        setCargandoUsuarios(false);
      }
    };
    obtenerUsuarios();
  }, []);

  // 2. Cargar las asistencias del usuario seleccionado cuando este cambie
  useEffect(() => {
    const obtenerAsistenciasDeUsuario = async () => {
      if (!usuarioSeleccionado) {
        setAsistencias([]);
        return;
      }
      setCargandoAsistencias(true);
      try {
        const response = await fetch(`/api/asistencias/usuario/${usuarioSeleccionado}`);
        const data = await response.json();
        setAsistencias(data);
      } catch (error) {
        console.error("Error al cargar asistencias del usuario:", error);
      } finally {
        setCargandoAsistencias(false);
      }
    };

    obtenerAsistenciasDeUsuario();
  }, [usuarioSeleccionado]);

  // Auxiliar para evaluar si el turno ya pasó
  const turnoYaTranscurrio = (fechaTurno, horarioTurno) => {
    const [dia, mes, anio] = fechaTurno.includes('/') ? fechaTurno.split('/') : fechaTurno.split('-');
    const [horas, minutos] = horarioTurno.replace('hs', '').trim().split(':');
    
    const anioCorrecto = dia.length === 4 ? dia : anio;
    const mesCorrecto = dia.length === 4 ? mes : mes - 1;
    const diaCorrecto = dia.length === 4 ? anio : dia;

    const fechaHoraTurno = new Date(anioCorrecto, mesCorrecto - 1, diaCorrecto, horas, minutos);
    return fechaHoraTurno < new Date();
  };

  const asistenciasFiltradas = asistencias.filter(asistencia => 
    asistencia.turno && turnoYaTranscurrio(asistencia.turno.fecha, asistencia.turno.horario)
  );

  return (
    <div style={{ maxWidth: '700px', margin: '2rem auto', padding: '1.5rem', fontFamily: 'sans-serif' }}>
      <h2 style={{ color: '#0d47a1', marginBottom: '1.5rem', textAlign: 'center' }}>Control de Asistencias de Alumnos</h2>

      {/* Selector de Usuario */}
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
            style={{
              padding: '12px',
              fontSize: '1rem',
              width: '100%',
              maxWidth: '400px',
              borderRadius: '8px',
              border: '1px solid #ccc',
              outline: 'none'
            }}
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

      {/* Listado de asistencias filtradas */}
      {usuarioSeleccionado && (
        <div>
          {cargandoAsistencias ? (
            <p style={{ textAlign: 'center' }}>Consultando historial...</p>
          ) : asistenciasFiltradas.length === 0 ? (
            <p style={{ textAlign: 'center', color: '#666' }}>Este alumno no registra asistencias en turnos transcurridos.</p>
          ) : (
            <ul style={{ listStyle: 'none', padding: 0 }}>
              {asistenciasFiltradas.map((asistencia) => (
                // 🌟 USAMOS EL MISMO COMPONENTE REUTILIZABLE ACÁ:
                <ItemAsistencia 
                  key={asistencia.id}
                  nombreTurno={asistencia.turno.nombre}
                  fecha={asistencia.turno.fecha}
                  horario={asistencia.turno.horario}
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