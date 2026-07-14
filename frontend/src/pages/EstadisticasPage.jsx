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

  const colores = [
    '#4dc9f6','#1E5Bf0','#0c0391','#1A2F50','#8d8d8c','#166a8f','#00a950','#58595b','#8549ba'
  ];

  const [tipo, setTipo] = useState('deportes');

  const verEstadisticas = async (nuevoTipo) => {
    setTipo(nuevoTipo);
    setMostrado(true);
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
  }, [datos]);

  return (
    <div style={{padding: '1rem'}}>
      <h2>Estadísticas</h2>
      {/* <p>Como Administrador, podés ver estadísticas por deporte o por turno. Tenés ambas opciones disponibles.</p> */}
      <div style={{display: 'flex', gap: '1rem', flexWrap: 'wrap', marginBottom: '1rem'}}>
        <button onClick={() => verEstadisticas('deportes')} style={{backgroundColor:'#1E5Bf0', color:'#fff', padding:'0.5rem 1rem', border:'none', borderRadius:4}}>Ver estadísticas de deportes</button>
        <button onClick={() => verEstadisticas('turnos')} style={{backgroundColor:'#6c757d', color:'#fff', padding:'0.5rem 1rem', border:'none', borderRadius:4}}>Ver estadísticas de turnos</button>
        <button onClick={() => verEstadisticas('asistencias')} style={{backgroundColor:'#0c0391', color:'#fff', padding:'0.5rem 1rem', border:'none', borderRadius:4}}>Ver estadísticas de asistencias</button>
        <button onClick={() => verEstadisticas('pagos')} style={{backgroundColor:'#166a8f', color:'#fff', padding:'0.5rem 1rem', border:'none', borderRadius:4}}>Ver estadísticas de pagos</button>
      </div>

      {cargando && <p>Cargando...</p>}
      {error && <p style={{color: 'red'}}>{error}</p>}

      {!cargando && !error && mostrado && (sinDatos || (datos && datos.length === 0)) && (
        <p>No hay estadísticas actuales sobre las {tipo === 'turnos' ? 'turnos' : tipo === 'asistencias' ? 'asistencias' : tipo === 'pagos' ? 'pagos' : 'deportes'}</p>
      )}

      {mostrado && !cargando && !error && !sinDatos && datos && datos.length > 0 && (
        <div style={{marginTop: '1rem'}}>
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

      <div style={{maxWidth: '380px', marginTop: '1rem'}}>
        <canvas ref={canvasRef} style={{width: '100%', height: '320px'}} />
      </div>

      {mostrado && !cargando && !error && !sinDatos && datos && datos.length > 0 && (
        <div style={{marginTop: '1rem'}}>
          <h3>Colores por {tipo === 'turnos' ? 'turno' : tipo === 'asistencias' || tipo === 'pagos' ? 'deporte' : 'deporte'}</h3>
          <ul style={{listStyle: 'none', padding: 0, margin: 0}}>
            {datos.map((d, index) => {
              const label = tipo === 'turnos'
                ? `${d.nombreTurno || d.NombreTurno || d.nombre || 'Turno'} (${d.NombreDeporte || d.nombreDeporte || d.nombre || ''})`
                : tipo === 'pagos'
                  ? (d.Categoria || d.categoria || d.categoria || 'Categoría')
                  : (d.nombreDeporte || d.NombreDeporte || d.nombre || d.nombreDeporte || 'Desconocido');
              const color = colores[index % colores.length];

              return (
                <li key={index} style={{display: 'flex', alignItems: 'center', marginBottom: '0.5rem'}}>
                  <span style={{width: '16px', height: '16px', backgroundColor: color, display: 'inline-block', borderRadius: '50%', marginRight: '0.75rem'}} />
                  <span>{label}</span>
                </li>
              );
            })}
          </ul>
        </div>
      )}
    </div>
  );
}