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

  const colores = [
    '#4dc9f6','#1E5Bf0','#0c0391','#1A2F50','#8d8d8c','#166a8f','#00a950','#58595b','#8549ba'
  ];

  const verEstadisticas = async () => {
    setMostrado(true);
    setCargando(true);
    setError(null);
    try {
      const resp = await apiClient.get('/Estadisticas/deportes');
      setDatos(resp.data || []);
    } catch (err) {
      setError('No se pudo cargar las estadísticas');
      setDatos([]);
    } finally {
      setCargando(false);
    }
  };

  // ESTE ES EL BLOQUE REEMPLAZADO
  useEffect(() => {
    if (!canvasRef.current) return;

    if (chartRef.current) {
      chartRef.current.destroy();
      chartRef.current = null;
    }

    if (datos && datos.length > 0) {
      // Agregamos d.nombreDeporte a las opciones
      const labels = datos.map(d => d.nombreDeporte || d.NombreDeporte || d.nombre || '');
      const values = datos.map(d => d.CantidadInscripciones ?? d.cantidadInscripciones ?? 0);
      const backgroundColor = datos.map((_, index) => colores[index % colores.length]);

      // Calculamos el total general sumando las inscripciones de todos los deportes
      const totalInscripciones = values.reduce((sum, current) => sum + current, 0);

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
                  
                  // Calculamos el porcentaje dinámicamente para el tooltip
                  const porcentaje = totalInscripciones > 0 
                    ? ((value / totalInscripciones) * 100).toFixed(1) 
                    : 0;

                  return ` ${label}: ${value} inscripciones (${porcentaje}%)`;
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
      <h2>Estadísticas de Deportes</h2>
      <p>Como Administrator, podés ver cuántas personas se inscriben por deporte.</p>
      <button onClick={verEstadisticas} className="btn btn-primary">Ver estadísticas de deportes</button>

      {cargando && <p>Cargando...</p>}
      {error && <p style={{color: 'red'}}>{error}</p>}

      {!cargando && !error && mostrado && datos && datos.length === 0 && (
        <p>No hay estadísticas actuales sobre los deportes</p>
      )}

      {mostrado && !cargando && !error && datos && datos.length > 0 && (
        <div style={{marginTop: '1rem'}}>
          <p>El gráfico representa la cantidad de inscripciones por deporte.</p>
        </div>
      )}

      <div style={{maxWidth: '300px', marginTop: '1rem'}}>
        <canvas ref={canvasRef} style={{width: '100%'}} />
      </div>

      {mostrado && !cargando && !error && datos && datos.length > 0 && (
        <div style={{marginTop: '1rem'}}>
          <h3>Colores por deporte</h3>
          <ul style={{listStyle: 'none', padding: 0, margin: 0}}>
          {datos.map((d, index) => {
  // Agregamos d.nombreDeporte también acá
  const label = d.nombreDeporte || d.NombreDeporte || d.nombre || 'Desconocido';
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