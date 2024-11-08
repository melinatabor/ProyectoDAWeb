﻿import { Link, useNavigate } from 'react-router-dom';
import '../styles/navbar.css';
import logo from '../assets/logo.jpg';
import { useLogin } from '../hooks/useLogin';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';

export const Navbar = () => {
    const navigate = useNavigate();
    const { user, isAuthenticated, logoutUser, getUserFromStorage } = useLogin();
    console.log(isAuthenticated);
    const currentUser = (user || getUserFromStorage()) && isAuthenticated;

    const handleLogout = async () => {
        try {
            const response = await fetch("/api/login/logout", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (response.ok) {
                logoutUser();
                navigate("/login");
            } else {
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
                console.log(msg);
            }
        } catch (error) {
            console.log("Error during logout:", error);
        }
    };

    const handleDownloadBackup = async () => {
        try {
            const response = await fetch("/api/backup/backup", {
                method: "GET",
                headers: {
                    "Content-Type": "application/octet-stream"
                }
            });

            if (response.ok) {
                const blob = await response.blob();

                const url = window.URL.createObjectURL(new Blob([blob]));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', 'base-de-datos.bak');
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            } else {
                const errorData = await response.json();
                alert(errorData.message);
                console.log(errorData.message);
            }
        } catch (error) {
            console.log("Error al descargar el archivo:", error);
        }
    };

    return (
        <nav className="navbar">
            <div className="navbar-container">
                <Link to="/" className="navbar-logo">
                    <img src={logo} alt="Distribuidora del Haras" />
                </Link>
                <ul className="navbar-menu">
                    <li><Link to="/#home">Inicio</Link></li>
                    <li><Link to="/productos">Productos</Link></li>
                    <li><Link to="/#about">Nosotros</Link></li>
                    <li><Link to="/#contact">Contacto</Link></li>
                    {
                        currentUser && currentUser.rol === 1 && <li><Link to="/bitacora">Bitacora</Link></li>
                    }
                    {
                        currentUser && currentUser.rol === 2 && <li><button onClick={handleDownloadBackup}>Backup BD</button></li>
                    }
                    {currentUser && currentUser.rol === 3 && (
                        <li><Link to="/carrito"><ShoppingCartIcon fontSize="large" /></Link></li>
                    )}
                </ul>
                <div className="navbar-user">
                    {isAuthenticated ? (
                        <>
                            <AccountCircleIcon className="user-icon" />
                            <span className="user-name">{currentUser.username}</span>
                            <button className="logout-button" onClick={handleLogout}>Cerrar Sesión</button>
                        </>
                    ) : (
                        <>
                            <Link to="/login" className="auth-link">Iniciar Sesión</Link>
                            <Link to="/registrar" className="auth-link">Registrarse</Link>
                        </>
                    )}
                </div>
            </div>
        </nav>
    );
}
