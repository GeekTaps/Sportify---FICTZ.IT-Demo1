import { useState, useEffect } from 'react'
import './App.css'

import BotonSimularDia11 from './components/BotonSimularDia11'
import HomePage from './pages/HomePage'
import DeportePage from './pages/DeportePage'
import ModificarDeportePage from './pages/ModificarDeportePage'
import CrearDeportePage from './pages/CrearDeportePage'
import TurnoPage from './pages/TurnoPage'
import CrearModificarTurnoPage from './pages/CrearModificarTurnoPage'
import RegistrarUsuarioPage from "./pages/RegistrarUsuarioPage"
import ModificarUsuarioPage from "./pages/ModificarUsuarioPage"
import ReservasPage from './pages/ReservasPage'
import PagosRegistrarPage from './pages/PagosRegistrarPage'
import MisPagosPage from './pages/MisPagosPage'
import LoginPage from './pages/LoginPage'
import VisualizarPagos from './pages/VisualizarPagos'
import ListadoPagosAdmin from './pages/ListadoPagosAdmin';
import OlvideMiContraseñaPage from './pages/OlvideMiContraseñaPage'
import ResetearContraseñaPage from './pages/ResetearContraseñaPage';
import RegistrarEmpleadoPage from './pages/RegistrarEmpleadoPage';
import SuspenderTurnoAdmin from "./pages/SuspenderTurnoAdmin"
import IngresarMailPage from './pages/IngresarMailPage'
import SimulacionDia11Page from './pages/SimulacionDia11Page'


import { BrowserRouter, Routes, Route, Link, NavLink, useNavigate } from "react-router-dom";
import { AuthProvider, AuthContext } from './context/AuthContext';
import { useContext } from 'react';

import logoLetras from './assets/logo_letras_sportify-no-bg.png';

function Navigation() {
    const { user, logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const navLinkClass = ({ isActive }) => (isActive ? "nav-active" : undefined);

    const handleLogout = () => {
        if (window.confirm("¿Seguro que querés cerrar sesión?")) {
            logout();
            navigate("/");
        }
    };

    return (
        <nav className="navbar">
            <div className="navbar-brand">
                <Link to="/">
                    <img src={logoLetras} alt="Sportify" className="navbar-logo" />
                </Link>
            </div>

            <div className="navbar-links">
                <NavLink to="/deportes" className={navLinkClass}>Deportes</NavLink>
                <NavLink to="/turnos" className={navLinkClass}>Turnos</NavLink>

                {user?.esAdmin && (
                    <>
                        <NavLink to="/turnos/crear" className={navLinkClass}>Crear Turno</NavLink>
                        <NavLink to="/deportes/crear" className={navLinkClass}>Crear Deporte</NavLink>
                        <NavLink to="/pagos/registrar" className={navLinkClass}>Registrar Pagos</NavLink>
                        <NavLink to="/pagos/visualizar" className={navLinkClass}>Visualizar Pagos</NavLink>
                        <NavLink to="/registrarEmpleado" className={navLinkClass}>Registrar empleado</NavLink>
                    </>
                )}

                {!user && (
                    <>
                        <NavLink to="/register" className={navLinkClass}>Registro</NavLink>
                        <Link
                            to="/login"
                            className="btn btn-secondary"
                            style={{ color: "var(--c-azul-pizarra)", padding: "0.45rem 1.1rem", fontSize: "0.95rem" }}
                        >
                            Iniciar Sesión
                        </Link>
                    </>
                )}

                {user && !user.esAdmin && (
                    <>
                        <NavLink to="/reservas" className={navLinkClass}>Mis Reservas</NavLink>
                        <NavLink to="/mis-pagos" className={navLinkClass}>Mis Pagos</NavLink>
                    </>
                )}
                {user && (
                    <NavLink to="/modificarUsuario" className={navLinkClass}>Mi Perfil</NavLink>
                )}

                {user && (
                    <div className="navbar-user-actions">
                        <span style={{ color: 'var(--c-cian-suave)', fontSize: '0.875rem' }}>
                            Hola, {user.nombreCompleto}
                        </span>
                        <button
                            onClick={handleLogout}
                            className="btn btn-outline"
                            style={{
                                borderColor: 'var(--c-cian-brillante)',
                                color: 'var(--c-cian-brillante)',
                                padding: '0.4rem 1rem',
                                fontSize: '0.875rem'
                            }}
                        >
                            Cerrar Sesión
                        </button>
                    </div>
                )}
            </div>
        </nav>
    );
}

function Footer() {
    return (
        <footer className="footer">
            <div className="footer-inner">
                <img src={logoLetras} alt="Sportify" className="footer-logo" />
                <p className="footer-tagline">Tu plataforma de deportes y reservas</p>
                <hr className="footer-divider" />
                <p className="footer-copy">© 2026 Sportify · FICTZ.IT Demo 1</p>
            </div>
        </footer>
    );
}

function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <div className="app-container">
                    <Navigation />

                    <main className="main-content">
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
                            <Route path="/pagos/registrar" element={<PagosRegistrarPage />} />
                            <Route path="/mis-pagos" element={<MisPagosPage />} />
                            <Route path="/reservas" element={<ReservasPage />} />
                            <Route path="/modificarUsuario" element={<ModificarUsuarioPage />} />
                            <Route path="/modificarUsuario/:id" element={<ModificarUsuarioPage />} />
                            <Route path="/pagos/visualizar" element={<VisualizarPagos />} />
                            <Route path="/pagos/admin/:usuarioId" element={<ListadoPagosAdmin />} />
                            <Route path="/olvide-mi-contrasena" element={<OlvideMiContraseñaPage />} />
                            <Route path="/reset-password" element={<ResetearContraseñaPage />} />
                            <Route path="/registrarEmpleado" element={<RegistrarEmpleadoPage />} />
                            <Route path="/suspender-turno-admin/:idTurno" element={<SuspenderTurnoAdmin />} /> 
                            <Route path="/ingresar-mail" element={<IngresarMailPage />} />
                            <Route path="/simular-dia-11" element={<SimulacionDia11Page />} />
                        </Routes>
                    </main>
                    
                    <BotonSimularDia11 />

                    <Footer />
                </div>
            </BrowserRouter>
        </AuthProvider>
    )
}

export default App;
