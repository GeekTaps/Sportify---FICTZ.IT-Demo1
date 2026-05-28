import { useState, useEffect } from 'react'
import './App.css'
// import { apiClient } from "./api/api-client"; // from development

import HomePage from './pages/HomePage'
import DeportePage from './pages/DeportePage'
import ModificarDeportePage from './pages/ModificarDeportePage'
import TurnoPage from './pages/TurnoPage'
import CrearModificarTurnoPage from './pages/CrearModificarTurnoPage'
import RegistrarUsuarioPage from "./pages/RegistrarUsuarioPage"
// import ModificarUsuarioPage from "./pages/ModificarUsuarioPage" // from development
// import ReservarClasePage from "./pages/ReservarClasePage" // from development

import ReservasPage from './pages/ReservasPage'
import LoginPage from './pages/LoginPage'

import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { AuthProvider, AuthContext } from './context/AuthContext';
import { useContext } from 'react';

// Importar el logo de la navbar
import logoLetras from './assets/logo_letras_sportify.png';

function Navigation() {
    const { user, logout } = useContext(AuthContext);

    return (
        <nav className="navbar">
            <div className="navbar-brand">
                <Link to="/">
                    <img src={logoLetras} alt="Sportify" className="navbar-logo" />
                </Link>
            </div>

            <div className="navbar-links">
                <Link to="/deportes">Deportes</Link>
                <Link to="/turnos">Turnos</Link>

                {!user && (
                    <>
                        <Link to="/register">Registro</Link>
                        <Link to="/login" className="btn btn-secondary" style={{ color: "var(--c-azul-pizarra)", textDecoration: "none", padding: "0.5rem 1rem" }}>Iniciar Sesión</Link>
                    </>
                )}

                {user && !user.esAdmin && (
                    <>
                        <Link to="/reservas">Mis Reservas</Link>
                        {/* <Link to={`/modificarUsuario/${user.id}`}> Modificar Datos </Link> // hola esto se implementa hoy jueves dejenlo aca mientras el boton de cerrar tambien dejenlo */}
                    </>
                )}
                
                {/* <button className="btn btn-danger" style={{marginLeft: "1rem"}}> Cerrar sesión </button> */}

                {user && (
                    <div className="navbar-user-actions">
                        <span style={{ color: 'var(--c-cian-suave)', fontSize: '0.9rem' }}>
                            Hola, {user.nombreCompleto}
                        </span>
                        <button onClick={logout} className="btn btn-outline" style={{ borderColor: 'var(--c-cian-brillante)', color: 'var(--c-cian-brillante)', padding: '0.4rem 1rem', fontSize: '0.9rem' }}>
                            Cerrar Sesión
                        </button>
                    </div>
                )}
            </div>
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

    /* 
    // from development branch:
    const [userDev, setUserDev] = useState(null);       
    const handleLogout = async () => {
        try {
            await apiClient.post("/auth/logout");
            setUserDev(null);
            console.log("logout ok"); // debug
        } catch (err) {
            console.error("logout error", err);
        }
    };
    */

    return (
        <AuthProvider>
            <BrowserRouter>
                <div className="app-container">
                    <Navigation />
                    
                    {/* Status bar */}
                    {backendMessage !== 'Conexión exitosa a SQLite.' && (
                        <div style={{ textAlign: 'center', background: '#fef3c7', padding: '5px', fontSize: '12px', color: '#92400e' }}>
                            {backendMessage}
                        </div>
                    )}

                    <main className="main-content">
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
                            
                            {/* Rutas de development: */}
                            {/* <Route path="/modificarUsuario/:id" element={<ModificarUsuarioPage />} /> */}
                            {/* <Route path="/reservar-clase" element={<ReservarClasePage />} /> */}
                        </Routes>
                    </main>
                </div>
            </BrowserRouter>
        </AuthProvider>
    )
}

export default App;