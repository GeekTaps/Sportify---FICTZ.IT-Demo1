import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from "../context/AuthContext";

function ConfirmarAsistencia() {
  const { alumnoId, turnoId } = useParams();
  const navigate = useNavigate();
  const [datosAlumno, setDatosAlumno] = useState(null);
  const [datosTurno, setDatosTurno] = useState(null);
  
  // 1. Unificamos el estado de carga usando cargandoDatos y setCargandoDatos
  const [cargandoDatos, setCargandoDatos] = useState(true);
  const [mensajeApi, setMensajeApi] = useState({ tipo: '', texto: '' });

  // 2. Traemos al usuario del contexto
  const { user, loading: loadingAuth } = useAuth();
  const usuarioLocal = JSON.parse(localStorage.getItem("user"));
  const currentUser = user || usuarioLocal;
  
  // Como definiste que temporalmente esEmpleado sea false, lo dejamos así por ahora
  const esEmpleado = currentUser ? currentUser.esEmpleado === true : false; 

  useEffect(() => {
    if (loadingAuth) return; // Esperamos a que el AuthContext termine de cargar

    if (!esEmpleado) {
      setCargandoDatos(false); // 👈 Corregido: antes decía setCargando
      return;
    }

    const obtenerDatosTurno = async () => {
      try {
        // Pedimos la info del turno
        const res = await fetch(`http://localhost:5266/api/Turnos/${turnoId}`);
        if (!res.ok) throw new Error("No se encontró el turno especificado.");
        
        const data = await res.json();
        setDatosTurno(data);

        // Pedimos la info del alumno
        const resAlumno = await fetch(`http://localhost:5266/api/Usuarios/${alumnoId}`);
        if (!resAlumno.ok) throw new Error("No se encontró el alumno.");
        const dataAlumno = await resAlumno.json();
        setDatosAlumno(dataAlumno);
      } catch (err) {
        setMensajeApi({ tipo: 'error', texto: err.message });
      } finally {
        setCargandoDatos(false); // 👈 Corregido: antes decía setCargando
      }
    };

    if (turnoId && alumnoId) obtenerDatosTurno();
  }, [turnoId, alumnoId, loadingAuth, esEmpleado]);

  // Si el AuthContext o la carga de datos de la API están procesando...
  if (loadingAuth || (esEmpleado && cargandoDatos)) {
    return (
      <p style={{ textAlign: 'center', marginTop: '3rem', color: 'gray', fontFamily: 'sans-serif' }}>
        Verificando credenciales y cargando datos...
      </p>
    );
  }

  // Vista restringida si no es un empleado de Sportify
  if (!esEmpleado) {
    return (
      <div style={{
        display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center',
        minHeight: '100vh', backgroundColor: '#f7fafc', fontFamily: 'sans-serif', padding: '2rem'
      }}>
        <div style={{
          maxWidth: '500px', textAlign: 'center', background: 'white', padding: '3rem 2rem',
          borderRadius: '16px', boxShadow: '0 10px 15px -3px rgba(0, 0, 0, 0.05)', borderTop: '5px solid #e53e3e'
        }}>
          <span style={{ fontSize: '4rem', marginBottom: '1rem', display: 'block' }}>🔒</span>
          <h2 style={{ color: '#2d3748', marginBottom: '1rem' }}>Acceso Restringido</h2>
          <p style={{ color: '#718096', fontSize: '1.1rem', lineHeight: '1.6', margin: '0 0 2rem 0' }}>
            Solo los empleados de <strong>Sportify</strong> pueden confirmar asistencias. 
            Si sos empleado, por favor iniciá sesión con tu cuenta autorizada.
          </p>
          
          <div style={{ display: 'flex', gap: '1rem', justifyContent: 'center' }}>
            {/* BOTÓN PARA IR AL LOGIN GUARDANDO LA RUTA ACTUAL */}
            <button 
              onClick={() => navigate('/login', { state: { redirigirA: window.location.pathname } })} 
              style={{
                backgroundColor: '#48bb78', color: 'white', border: 'none', padding: '0.75rem 1.5rem',
                borderRadius: '8px', fontSize: '1rem', cursor: 'pointer', fontWeight: 'bold'
              }}
            >
              Iniciar Sesión como Empleado
            </button>

            <button 
              onClick={() => navigate('/')} 
              style={{
                backgroundColor: '#a0aec0', color: 'white', border: 'none', padding: '0.75rem 1.5rem',
                borderRadius: '8px', fontSize: '1rem', cursor: 'pointer', fontWeight: 'bold'
              }}
            >
              Ir al Inicio
            </button>
          </div>
        </div>
      </div>
    );
  }

  // Confirmación de asistencia (Llamada PUT al backend)
  const registrarAsistencia = async () => {
    try {
      setMensajeApi({ tipo: 'info', texto: 'Modificando estado de asistencia...' });
      
      const res = await fetch(`http://localhost:5266/api/Asistencias/confirmar-presente`, {
        method: 'PUT', 
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          UsuarioId: alumnoId,
          TurnoId: turnoId     
        })
      });

      if (!res.ok) throw new Error("No se pudo actualizar la asistencia en el sistema.");

      setMensajeApi({ tipo: 'exito', texto: '¡Presente confirmado! Asistencia actualizada.' });
      
      setTimeout(() => navigate('/reservas'), 2500);

    } catch (err) {
      setMensajeApi({ tipo: 'error', texto: err.message });
    }
  };

  // ⚠️ Quitamos el bloque duplicado "if (cargando) {...}" que causaba el error de compilación.

  return (
    <div className="page-container" style={{ fontFamily: 'sans-serif', padding: '2rem', backgroundColor: '#f7fafc', minHeight: '100vh' }}>
      <main style={{ maxWidth: '900px', margin: '0 auto' }}>
        <h1 style={{ textAlign: 'center', color: '#2d3748', marginBottom: '2rem' }}>Validación de Pase QR</h1>

        <div style={{ display: 'flex', gap: '2rem', justifyContent: 'space-between', flexWrap: 'wrap' }}>
          
          {/* LADO IZQUIERDO: Info del Alumno */}
          <div style={{ flex: '1', minWidth: '300px', background: 'white', padding: '1.5rem', borderRadius: '12px', boxShadow: '0 4px 6px rgba(0,0,0,0.05)', borderTop: '4px solid #4299e1' }}>
            <h3 style={{ color: '#2b6cb0', marginTop: 0, marginBottom: '1rem' }}>Datos del Alumno</h3>
            <p style={{ margin: '0.5rem 0' }}><strong>Nombre y Apellido:</strong> {datosAlumno?.nombreCompleto}</p>
            <p style={{ margin: '0.5rem 0' }}><strong>DNI:</strong> {datosAlumno?.dni}</p>
            <p style={{ margin: '0.5rem 0' }}><strong>Email:</strong> {datosAlumno?.email}</p>
          </div>

          {/* LADO DERECHO: Info del Turno */}
          <div style={{ flex: '1', minWidth: '300px', background: 'white', padding: '1.5rem', borderRadius: '12px', boxShadow: '0 4px 6px rgba(0,0,0,0.05)', borderTop: '4px solid #48bb78' }}>
            <h3 style={{ color: '#2f855a', marginTop: 0, marginBottom: '1rem' }}>Detalles del Turno</h3>
            <p style={{ margin: '0.5rem 0' }}><strong>Actividad / Deporte:</strong> {datosTurno?.nombreTurno || 'Sin nombre de turno'}</p>
            {/* 💡 Corregido "nommbreProfesor" por "nombreProfesor" (suponiendo que viene así del back) */}
            <p style={{ margin: '0.5rem 0' }}><strong>Profesor:</strong> {datosTurno?.nombreProfesor || datosTurno?.nommbreProfesor}</p>
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

        {/* Botón de confirmación */}
        <div style={{ textAlign: 'center', marginTop: '2.5rem' }}>
          <button 
            onClick={registrarAsistencia}
            style={{
              backgroundColor: '#48bb78', 
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