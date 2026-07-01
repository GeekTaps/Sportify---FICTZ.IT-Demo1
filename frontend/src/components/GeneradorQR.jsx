import React, { useEffect, useState } from 'react';
import QRCode from 'react-qr-code';
import {useAuth} from '../context/AuthContext';

function GeneradorQR({idTurno}) {
  // Estado para guardar el valor del QR (empieza vacío)
  const [valorQR, setValorQR] = useState("");

  const {user} = useAuth(); // Obtenemos el usuario logueado 

    useEffect (() => {
    if (!user?.id || !idTurno) {
        if (!user?.id) {
            console.error("No se puede generar el QR: falta información del usuario logueado.");
        }
        if (!idTurno) {
            console.error("No se puede generar el QR: falta información del turno.");
        }
      return;
    }
    const UrlBase= "http://localhost:7001";
    const urlAsistencia= `${UrlBase}/confirmar-asistencia/${user.id}/${idTurno}`;
    setValorQR(urlAsistencia);
  }, [user, idTurno]);
    if (!valorQR) {
        return <p style={{ color: 'gray', textAlign: 'center' }}>Cargando código QR...</p>;
    }

  return (
    <div style={{ textAlign: 'center', marginTop: '1.5rem', padding: '1rem', border: '1px dashed var(--border)', borderRadius: '8px' }}>
      <h4 style={{ marginTop: 0, marginBottom: '10px' }}>Tu Pase de Asistencia</h4>
      <div style={{ background: 'white', padding: '12px', display: 'inline-block', borderRadius: '8px' }}>
        <QRCode 
          value={valorQR} 
          size={180} // Un tamaño un poco más compacto para el modal
          level="H"  
        />
      </div>
      <p style={{ marginTop: '8px', marginBottom: 0, fontSize: '0.9rem', color: 'gray' }}>
        Presentá este QR al ingresar a la clase.
      </p>
    </div>
  );
}

export default GeneradorQR;