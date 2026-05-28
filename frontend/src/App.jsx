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
import ReservasPage from './pages/ReservasPage'
import LoginPage from './pages/LoginPage'
import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { AuthProvider, AuthContext } from './context/AuthContext';
import { useContext } from 'react';

// Test de conexión backend (Con Antigravity, funciona :D)

function Navigation() {
    const { user, logout } = useContext(AuthContext);

    return (
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
            <Link to="/deportes">Deportes</Link>
            <Link to="/turnos">Turnos</Link>

            {!user && (
                <>
                    <Link to="/register">Registro</Link>
                    <Link to="/login">Iniciar Sesión</Link>
                </>
            )}

            {user && !user.esAdmin && (
                <>
                    <Link to="/reservas">Mis Reservas</Link>
                </>
            )}

            {user && (
                <button onClick={logout} style={{ marginLeft: "auto", cursor: "pointer", background: "none", border: "none", color: "red", fontWeight: "bold" }}>
                    Cerrar Sesión ({user.nombreCompleto})
                </button>
            )}
        </nav>
    );
}

function App() {
    const [backendMessage, setBackendMessage] = useState('Probando conexión con .NET...');

    useEffect(() => {
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
        <AuthProvider>
            <BrowserRouter>
                <Navigation />
                <p>{backendMessage}</p>

                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/register" element={<RegistrarUsuarioPage />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/deportes" element={<DeportePage />} />
                    <Route path="/deportes/modificar/:id" element={<ModificarDeportePage />} />
                    <Route path="/turnos" element={<TurnoPage />} />
                    <Route path="/turnos/crear" element={<CrearModificarTurnoPage />} />
                    <Route path="/turnos/modificar/:id" element={<CrearModificarTurnoPage />} />
                    <Route path="/reservas" element={<ReservasPage />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    )
}

export default App