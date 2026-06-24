import React, { useContext, useState, useEffect } from 'react';
import { AuthContext } from '../context/AuthContext';
import { apiClient } from '../api/api-client';
import BotonVerPagos from '../components/FrontUsuarios/BotonVerPagos';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';

function VisualizarPagos() {
  const { user } = useContext(AuthContext);
  const [usuarios, setUsuarios] = useState([]);

  useEffect(() => {
    const fetchUsuarios = async () => {
      try {
        const response = await apiClient.get('/usuarios');
        setUsuarios(response.data);
      } catch (error) {
        console.error('Error fetching usuarios:', error);
      }
    };

    if (user?.esAdmin) {
      fetchUsuarios();
    }
  }, [user]);

  return (
  <div>
    {usuarios
      .filter(usuario => !usuario.esAdmin)
      .map(usuario => (
        <div key={usuario.id}>
          <h3>{usuario.nombre}</h3>
          <p>{usuario.email}</p>
          <BotonVerPagos usuarioId={usuario.id} />
        </div>
      ))}
  </div>
);
}

export default VisualizarPagos;