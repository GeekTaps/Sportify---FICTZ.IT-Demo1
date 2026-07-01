import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

function ConfirmarAsistencia() {
  const { alumnoId, turnoId } = useParams();
  const navigate = useNavigate();

  const [datosTurno, setDatosTurno] = useState(null);
  const [cargando, setCargando] = useState(true);
  const [mensajeApi, setMensajeApi] = useState({ tipo: '', texto: '' });

  // 1. Buscamos los datos directamente en la entidad TURNO (No en reservas)
  useEffect(() => {
    const obtenerDatosTurno = async () => {
      try {
        // 🌟 CORRECCIÓN: Apuntamos al controlador de Turnos para traer la info de la clase
        const res = await fetch(`http://localhost:5266/api/Turnos/${turnoId}`);
        if (!res.ok) throw new Error("No se encontró el turno especificado.");
        
        const data = await res.json();
        setDatosTurno(data);
      } catch (err) {
        setMensajeApi({ tipo: 'error', texto: err.message });
      } finally {
        setCargando(false);
      }
    };

    if (turnoId) obtenerDatosTurno();
  }, [turnoId]);

  // 2. Modificamos la asistencia existente (Cambiamos el booleano a true)
  const registrarAsistencia = async () => {
    try {
      setMensajeApi({ tipo: 'info', texto: 'Modificando estado de asistencia...' });
      
      // 🌟 CORRECCIÓN: Le pegamos al módulo de tu backend encargado de actualizar el booleano
      const res = await fetch(`http://localhost:7001/api/Asistencias/confirmar-presente`, {
        method: 'PUT', // Usamos PUT o PATCH porque estamos MODIFICANDO una entidad que ya existe
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          UsuarioId: alumnoId, // Para que tu backend busque la fila de este alumno...
          TurnoId: turnoId     // ... en este turno específico.
        })
      });

      if (!res.ok) throw new Error("No se pudo actualizar la asistencia en el sistema.");

      setMensajeApi({ tipo: 'exito', texto: '¡Presente confirmado! Asistencia actualizada.' });
      
      // Redirección limpia después de registrar
      setTimeout(() => navigate('/reservas'), 2500);

    } catch (err) {
      setMensajeApi({ tipo: 'error', texto: err.message });
    }
  };

  if (cargando) {
    return <p style={{ textAlign: 'center', marginTop: '3rem', color: 'gray' }}>Validando datos del QR...</p>;
  }

  return (
    <div className="page-container" style={{ fontFamily: 'sans-serif', padding: '2rem', backgroundColor: '#f7fafc', minHeight: '100vh' }}>
      <main style={{ maxWidth: '900px', margin: '0 auto' }}>
        <h1 style={{ textAlign: 'center', color: '#2d3748', marginBottom: '2rem' }}>Validación de Pase QR</h1>

        <div style={{ display: 'flex', gap: '2rem', justifyContent: 'space-between', flexWrap: 'wrap' }}>
          
          {/* LADO IZQUIERDO: Info del Alumno que queremos pasar a Presente */}
          <div style={{ flex: '1', minWidth: '300px', background: 'white', padding: '1.5rem', borderRadius: '12px', boxShadow: '0 4px 6px rgba(0,0,0,0.05)', borderTop: '4px solid #4299e1' }}>
            <h3 style={{ color: '#2b6cb0', marginTop: 0, marginBottom: '1rem' }}>Datos del Alumno</h3>
            <p style={{ margin: '0.5rem 0' }}><strong>ID Alumno:</strong></p>
            <span style={{ fontSize: '0.85rem', color: '#718096', wordBreak: 'break-all' }}>{alumnoId}</span>
          </div>

          {/* LADO DERECHO: Info del Turno original */}
          <div style={{ flex: '1', minWidth: '300px', background: 'white', padding: '1.5rem', borderRadius: '12px', boxShadow: '0 4px 6px rgba(0,0,0,0.05)', borderTop: '4px solid #48bb78' }}>
            <h3 style={{ color: '#2f855a', marginTop: 0, marginBottom: '1rem' }}>Detalles del Turno</h3>
            {/* Adaptá estas propiedades a los nombres exactos que use tu objeto Turno en C# */}
            <p style={{ margin: '0.5rem 0' }}><strong>Actividad / Deporte:</strong> {datosTurno?.nombreActividad || datosTurno?.actividad || 'Clase de Gimnasio'}</p>
            <p style={{ margin: '0.5rem 0' }}><strong>Fecha:</strong> {datosTurno?.fecha}</p>
            <p style={{ margin: '0.5rem 0' }}><strong>Horario:</strong> {datosTurno?.horario || datosTurno?.horaInicio}</p>
            <p style={{ margin: '0.5rem 0', fontSize: '0.85rem', color: '#718096' }}><strong>ID Turno:</strong> {turnoId}</p>
          </div>

        </div>

        {/* Alertas */}
        {mensajeApi.texto && (
          <div style={{ 
            marginTop: '2rem', padding: '1rem', borderRadius: '8px', textAlign: 'center',
            backgroundColor: mensajeApi.tipo === 'error' ? '#fff5f5' : mensajeApi.tipo === 'exito' ? '#f0fff4' : '#ebf8ff',
            color: mensajeApi.tipo === 'error' ? '#c53030' : mensajeApi.tipo === 'exito' ? '#2f855a' : '#2b6cb0',
            border: `1px solid ${mensajeApi.tipo === 'error' ? '#feb2b2' : mensajeApi.tipo === 'exito' ? '#9ae6b4' : '#bee3f8'}`
          }}>
            {mensajeApi.texto}
          </div>
        )}

        {/* Botón de confirmación (Modifica el registro existente) */}
        <div style={{ textAlign: 'center', marginTop: '2.5rem' }}>
          <button 
            onClick={registrarAsistencia}
            style={{
              backgroundColor: '#48bb78', // Lo ponemos en verde para simular la acción de "dar el OK"
              color: 'white', border: 'none', padding: '0.75rem 2.5rem', fontSize: '1.1rem', fontWeight: 'bold', borderRadius: '8px', cursor: 'pointer', boxShadow: '0 4px 6px rgba(72, 187, 120, 0.3)'
            }}
          >
            Confirmar Presente (Modificar Asistencia)
          </button>
        </div>
      </main>
    </div>
  );
}

export default ConfirmarAsistencia;