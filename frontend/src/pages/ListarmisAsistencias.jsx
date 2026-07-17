import React, { useEffect, useState, useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import ItemAsistencia from '../components/ItemsAsistencia'; // 🌟 IMPORTADO DESDE COMPONENTS

const ListarmisAsistencias = () => {
  // ... (mismo useEffect y lógica que te pasé antes) ...

  return (
    <div style={{ maxWidth: '600px', margin: '2rem auto', padding: '1.5rem' }}>
      <h2 style={{ color: '#0d47a1', marginBottom: '1.5rem', textAlign: 'center' }}>Mi Historial de Asistencias</h2>
      
      {asistenciasFiltradas.length === 0 ? (
        <p style={{ textAlign: 'center', color: '#666' }}>No tenés asistencias registradas.</p>
      ) : (
        <ul style={{ listStyle: 'none', padding: 0 }}>
          {asistenciasFiltradas.map((asistencia) => (
            // 🌟 USAMOS EL COMPONENTE ACÁ:
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
  );
};

export default ListarmisAsistencias;