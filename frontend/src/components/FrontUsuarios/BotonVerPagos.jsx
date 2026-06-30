import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useContext } from 'react';

function BotonVerPagos( { usuarioId } ) {

    const navigate = useNavigate();
    return (
        <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '20px' }}>
            <button 
                onClick={() => navigate(`/pagos/admin/${usuarioId}`)}
                className="btn btn-primary"
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
                Ver sus pagos
            </button>
        </div>
    );
}

export default BotonVerPagos;