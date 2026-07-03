import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useContext } from 'react';
import { AuthContext } from '../context/AuthContext';

function BotonSimularDia11() {
  const navigate = useNavigate();
  const { user } = useContext(AuthContext);

  if (!user?.esAdmin) {
    return null;
  }

  return (
    <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '20px' }}>
      <button 
        onClick={() => navigate('/simular-dia-11')} 
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
        Simular Día 11
      </button>
    </div>
  );
}
export default BotonSimularDia11;