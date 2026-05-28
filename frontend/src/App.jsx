import { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'
// import { apiClient } from "./api/api-client"; // from development

import HomePage from './pages/HomePage'
import DeportePage from './pages/DeportePage'
import ModificarDeportePage from './pages/ModificarDeportePage'
import CrearDeportePage from './pages/CrearDeportePage'
import TurnoPage from './pages/TurnoPage'
import CrearModificarTurnoPage from './pages/CrearModificarTurnoPage'
import RegistrarUsuarioPage from "./pages/RegistrarUsuarioPage"
import ModificarUsuarioPage from "./pages/ModificarUsuarioPage"
// import ReservarClasePage from "./pages/ReservarClasePage" // from development

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
                alignItems: "center"
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
                    <Link to={`/modificarUsuario/${user.id}`}> Modificar Datos </Link>
                </>
            )}

            {/* <button > Cerrar sesión </button> */}

            {user && (
                <button
    onClick={() => {
        const confirmar = window.confirm("¿Seguro que querés cerrar sesión?");

        if (confirmar) {
            logout();
        }
    }}
    style={{
        marginLeft: "auto",
        cursor: "pointer",
        background: "none",
        border: "none",
        color: "red",
        fontWeight: "bold",
        padding: "8px"
    }}
>
    Cerrar Sesión ({user.nombreCompleto})
</button>
            )}
        </nav>
    );
}

function App() {
    

    return ( //esto es lo que se muestra en la interfaz.
         <AuthProvider>
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
                <Link to="/deportes/crear">Registrar deporte</Link>
                <Link to="/turnos">Turnos</Link>
                <Link to="/reservas">Reservas</Link>
            </nav>

            

                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/register" element={<RegistrarUsuarioPage />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/deportes" element={<DeportePage />} />
                <Route path="/deportes/crear" element={<CrearDeportePage />} />
                    <Route path="/deportes/modificar/:id" element={<ModificarDeportePage />} />
                    <Route path="/turnos" element={<TurnoPage />} />
                    <Route path="/turnos/crear" element={<CrearModificarTurnoPage />} />
                    <Route path="/turnos/modificar/:id" element={<CrearModificarTurnoPage />} />
                    <Route path="/reservas" element={<ReservasPage />} />

                    {/* Rutas de development: */}
                    <Route path="/modificarUsuario/:id" element={<ModificarUsuarioPage />} />
                    {/* <Route path="/reservar-clase" element={<ReservarClasePage />} /> */}
                </Routes>
            </BrowserRouter>
            </AuthProvider>
    )
}

export default App;