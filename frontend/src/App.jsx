import { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'
import HomePage from './pages/HomePage'
import DeportePage from './pages/DeportePage'
import ModificarDeportePage from './pages/ModificarDeportePage'
import TurnoPage from './pages/TurnoPage'
import CrearModificarTurnoPage from './pages/CrearModificarTurnoPage'
import RegistrarUsuarioPage from "./pages/RegistrarUsuarioPage"
import ReservarClasePage from "./pages/ReservarClasePage"
import ReservasPage from './pages/ReservasPage'
import { BrowserRouter, Routes, Route, Link } from "react-router-dom";

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

    return ( //esto es lo que se muestra en la interfaz.
        <BrowserRouter>
            <nav
                style={{
                    display: "flex",
                    gap: "15px",
                    padding: "10px",
                    background: "#eee",
                    marginBottom: "20px",
                }}
            >
                <Link to="/">Home</Link>
                <Link to="/register">Registro</Link>
                <Link to="/deportes">Deportes</Link>
                <Link to="/turnos">Turnos</Link>
                <Link to="/reservar-clase">Reservar Clase</Link>
                <Link to="/reservas">Mis Reservas</Link>
            </nav>

            <p>{backendMessage}</p>

            <Routes>
                <Route path="/" element={<HomePage />} />

                <Route path="/register" element={<RegistrarUsuarioPage />} />
                <Route path="/deportes" element={<DeportePage />} />
                <Route path="/deportes/modificar/:id" element={<ModificarDeportePage />} />
                <Route path="/turnos" element={<TurnoPage />} />
                <Route path="/turnos/crear" element={<CrearModificarTurnoPage />} />
                <Route path="/turnos/modificar/:id" element={<CrearModificarTurnoPage />} />
                <Route path="/reservar-clase" element={<ReservarClasePage />} />
                <Route path="/reservas" element={<ReservasPage />} />
            </Routes>
        </BrowserRouter>
    )
}

export default App