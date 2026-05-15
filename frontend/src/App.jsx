import { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'

// Test de conexión backend (Con Antigravity, funciona :D)

function App() {
  // contador automatico de clicks
  const [count, setCount] = useState(0)
  // primer elemento: estado inicial, segundo elemento: función que actualiza el estado
  const [backendMessage, setBackendMessage] = useState('Probando conexión con .NET...');

  useEffect(() => {
    // La URL del backend configurada en launchSettings.json (perfil http)
    fetch('http://localhost:5266/api/testconnection')
      .then(res => res.json())
      .then(data => {
        setBackendMessage(data.message);
      })
      .catch(err => {
        console.error(err);
        setBackendMessage('Error al conectar con el backend. Asegurate de que esté corriendo.');
      });
  }, []);

  return (
    <>
      <section id="center">
        <div className="hero">
          <img src={heroImg} className="base" width="170" height="179" alt="" />
          <img src={reactLogo} className="framework" alt="React logo" />
          <img src={viteLogo} className="vite" alt="Vite logo" />
        </div>
        <div>
          <h1>Get started</h1>
          <p>
            Edit <code>src/App.jsx</code> and save to test <code>HMR</code>
          </p>
        </div>

        <div style={{ marginTop: '2rem', padding: '1rem', border: '1px solid #ccc', borderRadius: '8px', backgroundColor: '#f9f9f9', color: '#333' }}>
          <h2>Test de Conexión Backend</h2>
          <p><strong>Estado:</strong> {backendMessage}</p>
        </div>

        <button
          type="button"
          className="counter"
          style={{ marginTop: '2rem' }}
          onClick={() => setCount((count) => count + 1)}
        >
          Count is {count}
        </button>
      </section>

      <div className="ticks"></div>
      <section id="spacer"></section>
    </>
  )
}

export default App
