import { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'
import HomePage from './pages/HomePage'

// Test de conexión backend (Con Antigravity, funciona :D)

function App() { // a partir de aca se agregan los componentes del frontend que van a aparecer en la interfaz con la que interactuara el usuario
  // contador automatico de clicks
  const [count, setCount] = useState(0)
  // primer elemento: estado inicial, segundo elemento: función que actualiza el estado
  const [backendMessage, setBackendMessage] = useState('Probando conexión con .NET...'); //NO BORRAR!!! es el mensaje que se muestra mientras se prueba la conexión con el backend, y luego se actualiza con la respuesta del backend o con un mensaje de error si no se pudo conectar.

  useEffect(() => { //NO TOCAR!!!! es la conexion con el backend de .net
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
      <HomePage />
    </>
  )
}

export default App
