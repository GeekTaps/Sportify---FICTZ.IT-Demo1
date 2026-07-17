import React, { useEffect, useState, useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import ItemAsistencia from '../components/ItemsAsistencia';

const ListarmisAsistencias = () => {
    const { user } = useContext(AuthContext);
    const [asistencias, setAsistencias] = useState([]);
    const [cargando, setCargando] = useState(true);

    useEffect(() => {
        if (!user) return;

        const obtenerAsistencias = async () => {
            try {
                const response = await fetch(`http://localhost:5266/api/asistencias/usuario/${user.id}`);
                if (!response.ok) throw new Error('Error al obtener asistencias');
                const data = await response.json();
                console.log(data); // 👀 para ver qué forma tiene cada objeto
                setAsistencias(data);
            } catch (error) {
                console.error("Error al cargar asistencias:", error);
            } finally {
                setCargando(false);
            }
        };

        obtenerAsistencias();
    }, [user]);

    if (cargando) {
        return <p style={{ textAlign: 'center', marginTop: '2rem' }}>Cargando...</p>;
    }

    return (
        <div style={{ maxWidth: '600px', margin: '2rem auto', padding: '1.5rem' }}>
            <h2 style={{ color: '#0d47a1', marginBottom: '1.5rem', textAlign: 'center' }}>Mi Historial de Asistencias</h2>

            {asistencias.length === 0 ? (
                <p style={{ textAlign: 'center', color: '#666' }}>No tenés asistencias registradas.</p>
            ) : (
                <ul style={{ listStyle: 'none', padding: 0 }}>
                    {asistencias.map((asistencia) => (
                        <ItemAsistencia
                            key={asistencia.id}
                            nombreTurno={asistencia.turno?.nombre ?? 'Sin turno'}
                            fecha={asistencia.turno?.fecha ?? '-'}
                            horario={asistencia.turno?.horario ?? '-'}
                            asiste={asistencia.asiste}
                        />
                    ))}
                </ul>
            )}
        </div>
    );
};

export default ListarmisAsistencias;