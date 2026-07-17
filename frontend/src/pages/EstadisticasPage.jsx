import React, { useRef, useEffect, useState } from 'react';
import { apiClient } from '../api/api-client';
import Chart from 'chart.js/auto';

export default function EstadisticasPage() {
  const canvasRef = useRef(null);
  const chartRef = useRef(null);
  const [datos, setDatos] = useState([]);
  const [cargando, setCargando] = useState(false);
  const [error, setError] = useState(null);
  const [mostrado, setMostrado] = useState(false);
  const [sinDatos, setSinDatos] = useState(false);

  const [showAsistenciasClase, setShowAsistenciasClase] = useState(false);
  const [turnosClase, setTurnosClase] = useState([]);
  const [cargandoTurnos, setCargandoTurnos] = useState(false);
  const [errorTurnos, setErrorTurnos] = useState(null);
  const [turnoSeleccionado, setTurnoSeleccionado] = useState('');
  const [asistenciasClase, setAsistenciasClase] = useState([]);
  const [cargandoAsistencias, setCargandoAsistencias] = useState(false);
  const [errorAsistencias, setErrorAsistencias] = useState(null);
  const [intentadoListarAsistencias, setIntentadoListarAsistencias] = useState(false);

  const colores = [
    '#4dc9f6', '#1E5Bf0', '#0c0391', '#1A2F50', '#8d8d8c', '#166a8f', '#00a950', '#58595b', '#8549ba'
  ];

  const [tipo, setTipo] = useState('deportes');

  const verEstadisticas = async (nuevoTipo) => {
    setTipo(nuevoTipo);
    setMostrado(true);
    setShowAsistenciasClase(false);
    setAsistenciasClase([]);
    setTurnoSeleccionado('');
    setSinDatos(false);
    setCargando(true);
    setError(null);

    try {
      const endpoint = nuevoTipo === 'turnos'
        ? '/Estadisticas/turnos'
        : nuevoTipo === 'asistencias'
          ? '/Estadisticas/asistencias'
          : nuevoTipo === 'pagos'
            ? '/Estadisticas/pagos'
            : '/Estadisticas/deportes';
      const resp = await apiClient.get(endpoint);
      setDatos(resp.data || []);
    } catch (err) {
      setError('No se pudo cargar las estadísticas');
      setDatos([]);
    } finally {
      setCargando(false);
    }
  };

  const abrirListadoAsistenciasClase = async () => {
    setShowAsistenciasClase(true);
    setTurnoSeleccionado('');
    setAsistenciasClase([]);
    setErrorTurnos(null);
    setErrorAsistencias(null);
    setIntentadoListarAsistencias(false);
    setCargandoTurnos(true);

    try {
      const respPasados = await apiClient.get('/Turnos/anteriores');
      const pasados = (respPasados.data || []).map((turno) => ({
        ...turno,
        estado: 'Pasada'
      }));

      if (pasados.length === 0) {
        throw new Error('No se encontraron clases para listar.');
      }

      setTurnosClase(pasados);
    } catch (err) {
      setErrorTurnos('No se pudieron cargar los turnos');
      setTurnosClase([]);
    } finally {
      setCargandoTurnos(false);
    }
  };

  const listarAsistenciasClase = async () => {
    setIntentadoListarAsistencias(true);
    if (!turnoSeleccionado) {
      setErrorAsistencias('Seleccione una clase primero');
      return;
    }

    setCargandoAsistencias(true);
    setErrorAsistencias(null);
    setAsistenciasClase([]);

    try {
      const resp = await apiClient.get(`/Asistencias/clase/${turnoSeleccionado}`);
      const asistencias = resp.data || [];
      setAsistenciasClase(asistencias);
      if (asistencias.length === 0) {
        setErrorAsistencias('No hay asistencias registradas en esta clase');
      }
    } catch (err) {
      setErrorAsistencias('No se pudieron cargar las asistencias de la clase');
      setAsistenciasClase([]);
    } finally {
      setCargandoAsistencias(false);
    }
  };

  useEffect(() => {
    if (!canvasRef.current) return;

    if (chartRef.current) {
      chartRef.current.destroy();
      chartRef.current = null;
    }

    if (datos && datos.length > 0) {
      const labels = datos.map(d =>
        tipo === 'pagos'
          ? d.Categoria || d.categoria || d.categoriaPago || ''
          : d.nombreTurno || d.nombreDeporte || d.NombreDeporte || d.nombre || ''
      );
      const values = datos.map(d =>
        tipo === 'pagos'
          ? (d.Cantidad ?? d.cantidad ?? 0)
          : (d.cantidadAsistencias ?? d.CantidadAsistencias ?? d.CantidadInscripciones ?? d.cantidadInscripciones ?? 0)
      );
      const backgroundColor = datos.map((_, index) => colores[index % colores.length]);
      const totalInscripciones = values.reduce((sum, current) => sum + current, 0);

      if (totalInscripciones === 0) {
        setSinDatos(true);
        return;
      } else {
        setSinDatos(false);
      }

      chartRef.current = new Chart(canvasRef.current.getContext('2d'), {
        type: 'pie',
        data: {
          labels,
          datasets: [{
            data: values,
            backgroundColor
          }]
        },
        options: {
          plugins: {
            legend: { position: 'bottom' },
            tooltip: {
              callbacks: {
                label: context => {
                  const label = context.label || '';
                  const value = context.parsed || 0;
                  const porcentaje = totalInscripciones > 0
                    ? ((value / totalInscripciones) * 100).toFixed(1)
                    : 0;
                  const suffix = tipo === 'asistencias' ? 'asistencias' : tipo === 'pagos' ? 'clases' : 'inscripciones';
                  return `${label}: ${value} ${suffix} (${porcentaje}%)`;
                }
              }
            }
          }
        }
      });
    }

    return () => {
      if (chartRef.current) {
        chartRef.current.destroy();
        chartRef.current = null;
      }
    };
  }, [datos, tipo]);

  return (
    <div style={{ padding: '1rem' }}>
      <h2>Informes y estadísticas</h2>
      <div style={{ display: 'flex', gap: '1rem', flexWrap: 'wrap', marginBottom: '1rem' }}>
        <button onClick={() => verEstadisticas('deportes')} style={{ backgroundColor: '#1E5Bf0', color: '#fff', padding: '0.5rem 1rem', border: 'none', borderRadius: 4 }}>Ver estadísticas de deportes</button>
        <button onClick={() => verEstadisticas('turnos')} style={{ backgroundColor: '#6c757d', color: '#fff', padding: '0.5rem 1rem', border: 'none', borderRadius: 4 }}>Ver estadísticas de turnos</button>
        <button onClick={() => verEstadisticas('asistencias')} style={{ backgroundColor: '#0c0391', color: '#fff', padding: '0.5rem 1rem', border: 'none', borderRadius: 4 }}>Ver estadísticas de asistencias</button>
        <button onClick={() => verEstadisticas('pagos')} style={{ backgroundColor: '#166a8f', color: '#fff', padding: '0.5rem 1rem', border: 'none', borderRadius: 4 }}>Ver estadísticas de pagos</button>
        <button onClick={abrirListadoAsistenciasClase} style={{ backgroundColor: '#0056b3', color: '#fff', padding: '0.5rem 1rem', border: 'none', borderRadius: 4 }}>Listados de asistencias</button>
      </div>

      {cargando && <p>Cargando...</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {!cargando && !error && mostrado && (sinDatos || (datos && datos.length === 0)) && (
        <p>No hay estadísticas actuales sobre las {tipo === 'turnos' ? 'turnos' : tipo === 'asistencias' ? 'asistencias' : tipo === 'pagos' ? 'pagos' : 'deportes'}.</p>
      )}

      {mostrado && !cargando && !error && !sinDatos && datos && datos.length > 0 && (
        <div style={{ marginTop: '1rem' }}>
          <p>
            {tipo === 'turnos'
              ? 'El gráfico representa la cantidad de inscripciones por turno.'
              : tipo === 'asistencias'
                ? 'El gráfico representa el porcentaje de asistencias por deporte.'
                : tipo === 'pagos'
                  ? 'El gráfico representa el porcentaje de clases pagadas (seña pendiente vs pagado completo).'
                  : 'El gráfico representa la cantidad de inscripciones por deporte.'}
          </p>
        </div>
      )}

      <div style={{ maxWidth: '380px', marginTop: '1rem' }}>
        <canvas ref={canvasRef} style={{ width: '100%', height: '320px' }} />
      </div>

      {mostrado && !cargando && !error && !sinDatos && datos && datos.length > 0 && (
        <div style={{ marginTop: '1rem' }}>
          <h3>Colores por {tipo === 'turnos' ? 'turno' : tipo === 'asistencias' || tipo === 'pagos' ? 'deporte' : 'deporte'}</h3>
          <ul style={{ listStyle: 'none', padding: 0, margin: 0 }}>
            {datos.map((d, index) => {
              const label = tipo === 'turnos'
                ? `${d.nombreTurno || d.NombreTurno || d.nombre || 'Turno'} (${d.NombreDeporte || d.nombreDeporte || d.nombre || ''})`
                : tipo === 'pagos'
                  ? (d.Categoria || d.categoria || 'Categoría')
                  : (d.nombreDeporte || d.NombreDeporte || d.nombre || 'Desconocido');
              const color = colores[index % colores.length];

              return (
                <li key={index} style={{ display: 'flex', alignItems: 'center', marginBottom: '0.5rem' }}>
                  <span style={{ width: '16px', height: '16px', backgroundColor: color, display: 'inline-block', borderRadius: '50%', marginRight: '0.75rem' }} />
                  <span>{label}</span>
                </li>
              );
            })}
          </ul>
        </div>
      )}

      {showAsistenciasClase && (
        <div style={{ marginTop: '2rem', padding: '1rem', border: '1px solid #d1d5db', borderRadius: '8px', maxWidth: '700px' }}>
          <h3>Listado de asistencias por clase</h3>
          <p>Seleccione una clase y presione listar asistencias para ver las asistencias registradas.</p>

          {cargandoTurnos && <p>Cargando clases...</p>}
          {errorTurnos && <p style={{ color: 'red' }}>{errorTurnos}</p>}

          {!cargandoTurnos && !errorTurnos && (
            <div style={{ display: 'flex', gap: '0.75rem', flexWrap: 'wrap', alignItems: 'center' }}>
              <select
                value={turnoSeleccionado}
                onChange={(e) => setTurnoSeleccionado(e.target.value)}
                style={{ padding: '0.6rem', flex: '1 1 320px', borderRadius: 4, border: '1px solid #ccc' }}
              >
                <option value="">Seleccionar clase</option>
                {turnosClase.map((turno) => {
                  const fechaValor = turno.Fecha || turno.fecha;
                  const horaValor = turno.horaInicio || turno.horaInicio;
                  const labelFecha = fechaValor ? new Date(fechaValor).toLocaleDateString() : '';
                  const labelTurno = turno.nombreTurno || turno.nombre || `${labelFecha ? `${labelFecha} - ` : ''}${horaValor || ''}`;

                  return (
                    <option key={turno.id || turno.Id} value={turno.id || turno.Id}>
                      {labelTurno} {turno.estado ? `(${turno.estado})` : ''}
                    </option>
                  );
                })}
              </select>
              <button
                onClick={listarAsistenciasClase}
                style={{ backgroundColor: '#0c0391', color: '#fff', padding: '0.6rem 1rem', border: 'none', borderRadius: 4 }}
              >
                Listar asistencias
              </button>
            </div>
          )}

          {cargandoAsistencias && <p>Cargando asistencias...</p>}
          {intentadoListarAsistencias && errorAsistencias && <p style={{ color: 'red' }}>{errorAsistencias}</p>}

          {intentadoListarAsistencias && !cargandoAsistencias && !errorAsistencias && turnoSeleccionado && asistenciasClase.length === 0 && (
            <p>No se encontraron asistencias registradas para la clase seleccionada.</p>
          )}

          {asistenciasClase.length > 0 && (
            <div style={{ marginTop: '1rem' }}>
              <h4>Asistencias registradas</h4>
              <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                  <tr>
                    <th style={{ borderBottom: '1px solid #ccc', textAlign: 'left', padding: '0.5rem' }}>Alumno / Usuario</th>
                    <th style={{ borderBottom: '1px solid #ccc', textAlign: 'left', padding: '0.5rem' }}>Presente</th>
                  </tr>
                </thead>
                <tbody>
                  {asistenciasClase.map((a) => (
                    <tr key={a.Id || a.id}>
                      <td style={{ borderBottom: '1px solid #eee', padding: '0.5rem' }}>{a.Usuario || a.usuario || 'Desconocido'}</td>
                      <td style={{ borderBottom: '1px solid #eee', padding: '0.5rem' }}>{(a.Presente ?? a.presente) ? 'Sí' : 'No'}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}
    </div>
  );
}
    