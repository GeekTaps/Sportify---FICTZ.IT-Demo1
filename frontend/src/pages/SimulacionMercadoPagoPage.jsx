import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import mpLogo from '../assets/logo-mercadopago.png';

function SimulacionMercadoPagoPage() {
  const { user } = useContext(AuthContext);
  const location = useLocation();
  const navigate = useNavigate();

  const [tipo, setTipo] = useState('');
  const [idTurno, setIdTurno] = useState('');
  const [monto, setMonto] = useState(0);
  const [email, setEmail] = useState('');

  const [numeroTarjeta, setNumeroTarjeta] = useState('');
  const [nombreTitular, setNombreTitular] = useState('');
  const [vencimiento, setVencimiento] = useState('');
  const [cvc, setCvc] = useState('');
  const [emailInput, setEmailInput] = useState('');
  
  const [procesando, setProcesando] = useState(false);
  const [formError, setFormError] = useState('');
  const [resultadoFinal, setResultadoFinal] = useState(null); // 'exitoso', 'rechazado', 'error_interno'

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const tipoParam = params.get('tipo');
    const idTurnoParam = params.get('idTurno');
    const montoParam = params.get('monto');
    const emailParam = params.get('email');

    if (!tipoParam || !idTurnoParam || !emailParam) {
      alert("Faltan parámetros para el pago.");
      navigate("/turnos", { replace: true });
      return;
    }

    setTipo(tipoParam);
    setIdTurno(idTurnoParam);
    setMonto(montoParam ? parseFloat(montoParam) : 0);
    setEmail(emailParam);
    setEmailInput(emailParam); // Pre-fill email with logged-in user email
  }, [location, navigate]);

  const handleVencimientoChange = (e) => {
    let val = e.target.value.replace(/\D/g, ''); // Remove non-digits
    if (val.length >= 3) {
      val = val.substring(0, 2) + '/' + val.substring(2, 4);
    }
    setVencimiento(val);
  };

  const handlePagar = async (e) => {
    e.preventDefault();
    setFormError('');

    if (!numeroTarjeta || !nombreTitular || !vencimiento || !cvc || !emailInput) {
      setFormError('Debes completar todos los campos');
      return;
    }

    setProcesando(true);

    // Simulador de error por tarjeta rechazada
    if (numeroTarjeta.replace(/\s+/g, '') === '1234567890123456') {
      setTimeout(() => {
        setProcesando(false);
        setResultadoFinal('rechazado');
      }, 1500);
      return;
    }

    try {
      let endpoint = '';
      let bodyData = { idTurno, email: emailInput };

      if (tipo === 'reserva') {
        endpoint = 'http://localhost:5266/api/pagos/pagar-sena';
      } else if (tipo === 'abono') {
        endpoint = 'http://localhost:5266/api/abonos/procesar-pago-local';
        bodyData = { ...bodyData, montoPago: monto };
      } else if (tipo === 'cuota_abono') {
        endpoint = 'http://localhost:5266/api/abonos/pagar-cuota-local';
        bodyData = { idTurno, email: emailInput };
      } else if (tipo === 'confirmacion') {
        endpoint = 'http://localhost:5266/api/pagos/confirmar-reserva';
        bodyData = { idReserva: idTurno, email: emailInput };
      }

      const response = await fetch(endpoint, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(bodyData)
      });

      if (response.ok) {
        setResultadoFinal('exitoso');
      } else {
        const errorData = await response.json();
        console.error(errorData);
        setResultadoFinal('error_interno');
      }
    } catch (error) {
      console.error(error);
      setResultadoFinal('error_interno');
    } finally {
      setProcesando(false);
    }
  };

  const handleVolver = () => {
    if (tipo === 'confirmacion' || tipo === 'cuota_abono') {
      navigate(`/reservas?pago=${resultadoFinal}`, { replace: true });
    } else {
      navigate(`/turnos?${tipo === 'reserva' ? 'pago' : 'abono'}=${resultadoFinal}`, { replace: true });
    }
  };

  if (resultadoFinal) {
    return (
      <div style={{ backgroundColor: '#f5f5f5', minHeight: '100vh', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', fontFamily: '"Proxima Nova", -apple-system, "Helvetica Neue", Helvetica, Roboto, Arial, sans-serif' }}>
        <div style={{ backgroundColor: '#fff', borderRadius: '8px', boxShadow: '0 1px 2px 0 rgba(0,0,0,.15)', width: '100%', maxWidth: '450px', padding: '40px', textAlign: 'center' }}>
          {resultadoFinal === 'exitoso' && (
            <>
              <div style={{ backgroundColor: '#00a650', color: 'white', borderRadius: '50%', width: '80px', height: '80px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '40px', margin: '0 auto 24px' }}>✓</div>
              <h2 style={{ color: '#333', margin: '0 0 16px 0', fontSize: '24px' }}>¡Pago exitoso!</h2>
              <p style={{ color: '#666', marginBottom: '32px' }}>Tu pago se ha procesado correctamente y la operación está confirmada.</p>
            </>
          )}
          {resultadoFinal === 'rechazado' && (
            <>
              <div style={{ backgroundColor: '#f23d4f', color: 'white', borderRadius: '50%', width: '80px', height: '80px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '40px', margin: '0 auto 24px' }}>✕</div>
              <h2 style={{ color: '#333', margin: '0 0 16px 0', fontSize: '24px' }}>Pago rechazado</h2>
              <p style={{ color: '#666', marginBottom: '32px' }}>La tarjeta no tiene fondos suficientes o ha sido rechazada.</p>
            </>
          )}
          {resultadoFinal === 'error_interno' && (
            <>
              <div style={{ backgroundColor: '#f23d4f', color: 'white', borderRadius: '50%', width: '80px', height: '80px', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '40px', margin: '0 auto 24px' }}>!</div>
              <h2 style={{ color: '#333', margin: '0 0 16px 0', fontSize: '24px' }}>Ocurrió un error</h2>
              <p style={{ color: '#666', marginBottom: '32px' }}>Hubo un problema al procesar tu pago en el servidor.</p>
            </>
          )}
          <button 
            onClick={handleVolver}
            style={{ 
              backgroundColor: 'rgba(65,137,230,.15)', 
              color: '#3483fa', 
              padding: '16px', 
              border: 'none', 
              borderRadius: '6px', 
              fontSize: '16px', 
              fontWeight: 600, 
              cursor: 'pointer',
              width: '100%',
              transition: 'background-color 0.2s'
            }}
          >
            Volver a la página
          </button>
        </div>
      </div>
    );
  }

  const inputStyle = {
    width: '100%', 
    padding: '14px', 
    border: '1px solid #e6e6e6', 
    borderRadius: '6px', 
    fontSize: '16px', 
    boxSizing: 'border-box',
    color: '#333',
    backgroundColor: '#fff',
    transition: 'border-color .2s'
  };

  const labelStyle = {
    display: 'block', 
    fontSize: '14px', 
    color: '#999', 
    marginBottom: '6px'
  };

  return (
    <div style={{ backgroundColor: '#f5f5f5', minHeight: '100vh', display: 'flex', flexDirection: 'column', fontFamily: '"Proxima Nova", -apple-system, "Helvetica Neue", Helvetica, Roboto, Arial, sans-serif' }}>
      
      {/* Mercado Pago Navbar */}
      <header style={{ backgroundColor: '#ffe600', padding: '12px 24px', display: 'flex', alignItems: 'center', height: '60px', boxSizing: 'border-box' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
          <img src={mpLogo} alt="Mercado Pago" style={{ height: '32px', objectFit: 'contain' }} onError={(e) => { e.target.style.display = 'none'; }} />
        </div>
      </header>

      <main style={{ maxWidth: '1000px', width: '100%', margin: '0 auto', padding: '40px 20px', boxSizing: 'border-box' }}>
        
        <h1 style={{ color: '#333', fontSize: '24px', fontWeight: 600, margin: '0 0 24px 0' }}>Revisá tu pago</h1>

        <div style={{ display: 'flex', gap: '32px', flexWrap: 'wrap', alignItems: 'flex-start' }}>
          
          {/* Left Column: Medio de pago */}
          <div style={{ flex: '1 1 500px' }}>
            <h2 style={{ fontSize: '18px', color: '#333', fontWeight: 600, marginBottom: '16px' }}>Medio de pago</h2>
            
            <form id="payment-form" onSubmit={handlePagar} style={{ backgroundColor: '#fff', borderRadius: '8px', padding: '32px', boxShadow: '0 1px 2px 0 rgba(0,0,0,.15)' }}>
              <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
                
                <div>
                  <label style={labelStyle}>Número de tarjeta</label>
                  <input 
                    type="text" 
                    placeholder="Ingresa 1234567890123456 para simular fallo"
                    value={numeroTarjeta}
                    onChange={(e) => setNumeroTarjeta(e.target.value)}
                    style={inputStyle}
                  />
                </div>

                <div>
                  <label style={labelStyle}>Nombre y apellido</label>
                  <input 
                    type="text" 
                    placeholder="Como aparece en la tarjeta"
                    value={nombreTitular}
                    onChange={(e) => setNombreTitular(e.target.value)}
                    style={inputStyle}
                  />
                </div>

                <div style={{ display: 'flex', gap: '16px' }}>
                  <div style={{ flex: 1 }}>
                    <label style={labelStyle}>Vencimiento</label>
                    <input 
                      type="text" 
                      placeholder="MM/AA"
                      value={vencimiento}
                      onChange={handleVencimientoChange}
                      maxLength="5"
                      style={inputStyle}
                    />
                  </div>
                  <div style={{ flex: 1 }}>
                    <label style={labelStyle}>Código de seguridad</label>
                    <input 
                      type="text" 
                      placeholder="CVC"
                      value={cvc}
                      onChange={(e) => setCvc(e.target.value)}
                      maxLength="4"
                      style={inputStyle}
                    />
                  </div>
                </div>

                <div>
                  <label style={labelStyle}>E-mail del titular</label>
                  <input 
                    type="email" 
                    placeholder="Ej: nombre@email.com"
                    value={emailInput}
                    onChange={(e) => setEmailInput(e.target.value)}
                    style={inputStyle}
                  />
                </div>
              </div>
            </form>
          </div>

          {/* Right Column: Detalles del pago */}
          <div style={{ flex: '1 1 300px', maxWidth: '400px' }}>
            
            <div style={{ backgroundColor: '#fff', borderRadius: '8px', padding: '32px', boxShadow: '0 1px 2px 0 rgba(0,0,0,.15)' }}>
              <h2 style={{ fontSize: '18px', color: '#333', fontWeight: 600, marginBottom: '24px' }}>Detalles del pago</h2>
              
              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '24px', color: '#666', fontSize: '16px' }}>
                <span>{tipo === 'reserva' ? 'Seña de reserva de clase' : 'Abono Mensual'}</span>
                <span>${monto}</span>
              </div>

              <hr style={{ border: 'none', borderTop: '1px solid #e6e6e6', margin: '24px 0' }} />

              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '32px', color: '#333', fontSize: '18px', fontWeight: 600 }}>
                <span>Pagás</span>
                <span>1x ${monto}</span>
              </div>

              <div style={{ textAlign: 'center', fontSize: '12px', color: '#999', marginBottom: '16px' }}>
                Al pagar, aceptás los Términos y condiciones<br/>de Mercado Pago.
              </div>

              {formError && (
                <div style={{ color: '#f23d4f', fontSize: '14px', fontWeight: 600, textAlign: 'center', marginBottom: '16px' }}>
                  {formError}
                </div>
              )}

              <button 
                type="submit"
                form="payment-form"
                disabled={procesando}
                style={{ 
                  backgroundColor: '#3483fa', 
                  color: '#fff', 
                  padding: '16px', 
                  border: 'none', 
                  borderRadius: '6px', 
                  fontSize: '16px', 
                  fontWeight: 600, 
                  cursor: procesando ? 'not-allowed' : 'pointer',
                  width: '100%',
                  transition: 'background-color 0.2s',
                  marginBottom: '16px'
                }}
              >
                {procesando ? 'Procesando...' : 'Pagar'}
              </button>

              <div style={{ textAlign: 'center', fontSize: '12px', color: '#999', display: 'flex', justifyContent: 'center', alignItems: 'center', gap: '4px' }}>
                <span>🔒</span> Pago seguro
              </div>
              
              <button 
                type="button"
                onClick={() => navigate('/turnos')}
                disabled={procesando}
                style={{ 
                  backgroundColor: 'transparent', 
                  color: '#3483fa', 
                  padding: '12px', 
                  border: 'none', 
                  fontSize: '14px', 
                  cursor: 'pointer',
                  width: '100%',
                  marginTop: '16px'
                }}
              >
                Cancelar y volver a Sportify
              </button>

            </div>
          </div>
        </div>
      </main>

      <footer style={{ marginTop: 'auto', padding: '24px', textAlign: 'center', fontSize: '12px', color: '#999' }}>
        Procesado por Mercado Pago | Protegido por reCAPTCHA - Privacidad - Condiciones
      </footer>

    </div>
  );
}

export default SimulacionMercadoPagoPage;
