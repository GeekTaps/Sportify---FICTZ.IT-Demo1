import React from 'react';

const ItemAsistencia = ({ nombreTurno, fecha, horario, asiste }) => {
  return (
    <li 
      style={{
        display: 'flex',
        justifyContent: 'space-between',
        padding: '12px 20px',
        borderBottom: '1px solid #eee',
        fontSize: '1.1rem',
        alignItems: 'center'
      }}
    >
      <span style={{ fontWeight: '500', color: '#333' }}>
        {nombreTurno} ({fecha} - {horario})
      </span>
      <span style={{ 
        fontWeight: 'bold', 
        color: asiste === 'V' ? '#2e7d32' : '#c62828'
      }}>
        {asiste === 'V' ? 'Asistió' : 'Ausente'}
      </span>
    </li>
  );
};

export default ItemAsistencia;