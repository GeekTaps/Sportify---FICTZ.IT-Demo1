import React from 'react';
import { useNavigate } from 'react-router-dom';

function BotonMisReservas() {
  const navigate = useNavigate();

  return (
    <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '20px' }}>
      <button 
        onClick={() => navigate('/reservas')} 
        style={{ 
          padding: '12px 24px', 
          backgroundColor: '#007bff', 
          color: 'white', 
          border: 'none', 
          borderRadius: '5px', 
          cursor: 'pointer',
          fontSize: '16px',
          fontWeight: 'bold'
        }}
      >
        Ver Mis Reservas
      </button>
    </div>
  );
}

export default BotonMisReservas;
