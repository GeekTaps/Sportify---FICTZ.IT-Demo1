 import React, { useState } from 'react';
import QRCode from 'react-qr-code';

function GeneradorQR() {
  // Estado para guardar el valor del QR (empieza vacío)
  const [valorQR, setValorQR] = useState("");

  const manejarGeneracion = () => {
    // Por ahora usamos un texto hardcodeado de prueba.
    // Más adelante, acá haremos el fetch a tu API de .NET 10.
    const textoPrueba = "Asistencia-Clase123-Usuario456";
    setValorQR(textoPrueba);
  };

  return (
    <div style={{ textAlign: 'center', marginTop: '20px' }}>
      <h3>Generar QR de Asistencia</h3>
      
      <button onClick={manejarGeneracion} style={{ padding: '10px 20px', marginBottom: '20px' }}>
        Generar Código QR
      </button>

      {/* Si valorQR tiene texto, mostramos el QR */}
      {valorQR && (
        <div style={{ background: 'white', padding: '16px', display: 'inline-block', borderRadius: '8px' }}>
          <QRCode 
            value={valorQR} 
            size={256} // Tamaño en píxeles
            level="H"  // Nivel de corrección de errores (H es el más alto, ideal para escanear de pantallas)
          />
          <p style={{ marginTop: '10px', color: '#333' }}>Mostrá este código al empleado</p>
        </div>
      )}
    </div>
  );
}

export default GeneradorQR;