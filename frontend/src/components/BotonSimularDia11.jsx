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
    <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '20px', gap: '15px' }}>
      <button
        onClick={() => navigate('/simular-dia-10')}
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
        Simular Día 10
      </button>

      <button
        onClick={async () => {
          if (!window.confirm("¿Estás seguro de que querés simular el Día 11? Esto cancelará abonos no pagados y reseteará estados.")) return;
          try {
            const response = await fetch("http://localhost:5266/api/simulacion/dia11", { method: "POST" });
            if (response.ok) {
              alert("Simulación del Día 11 completada con éxito.");
            } else {
              alert("Ocurrió un error al simular el Día 11.");
            }
          } catch (error) {
            console.error(error);
            alert("Error de conexión al simular el Día 11.");
          }
        }}
        style={{
          padding: '12px 24px',
          backgroundColor: '#dc3545',
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