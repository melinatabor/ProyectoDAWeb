import { Link, useNavigate } from 'react-router-dom';
import '../styles/navbar.css';
import logo from '../assets/logo.jpg';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import { useSession } from '../hooks/useSession';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { useTranslations } from '../hooks/useTranslations';
import { useState, useEffect } from 'react';
import { usePermissions } from '../hooks/usePermissions';

export const Navbar = () => {
    const navigate = useNavigate();
    const { user, clear } = useSession();
    const [languages, setLanguages] = useState(null);
    const { fetchLanguage, clearLanguage, setLanguage, gettext } = useTranslations();
    const { permissions, fetchPermissions } = usePermissions(user?.id);

    const handleLogout = async () => {
        try {
            const response = await fetch("/api/login/logout", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (response.ok) {
                clear();
                navigate("/login");
                window.location.reload();
            } else {
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
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


    const handleClick = (id) => {
        clearLanguage();
        setLanguage(id);
    };

    useEffect(() => {
        const getIdiomas = async () => {
            setTimeout(async () => { setLanguages(await fetchLanguage()) }, 3000);
        };
        getIdiomas();
    }, []);

    useEffect(() => {
        if (user) {
            fetchPermissions();
        }

    }, [user]);

    return (
        <nav className="navbar">
            <div className="navbar-container">
                <Link to="/" className="navbar-logo">
                    <img src={logo} alt="Distribuidora del Haras" />
                </Link>
                <ul className="navbar-menu">
                    <li><Link to="/#home">{gettext('tagInicio')}</Link></li>
                    <li><Link to="/productos">{gettext('tagProductos')}</Link></li>
                    <li><Link to="/#about">{gettext('tagNosotros')}</Link></li>
                    <li><Link to="/#contact">{gettext('tagContacto')}</Link></li>
                    
                    {
                        user && permissions && permissions.map((permiso) => {
                            if (permiso.nombre === 'Admin') {
                                return <>
                                    < li > <Link to="/bitacora">{gettext('tagBitacora')}</Link></li>
                                    < li > <Link to="/permisos">{gettext('tagPermiso')}</Link></li>
                                </>
                            }
                            if (permiso.nombre === 'Master') {
                                return <>
                                    <li><button className="btn btn-primary" onClick={handleDownloadBackup}>{gettext('tagBackup')}</button></li>
                                </>
                            }
                            if (permiso.nombre === 'Cliente') return <li><Link to="/carrito"><ShoppingCartIcon fontSize="large" /></Link></li>
                        })
                    }
                    <li className="nav-item dropdown">
                        <button className="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                            Idioma
                        </button>
                        <ul className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                            {languages &&
                                languages.map((idioma) => (
                                    <li><button className="dropdown-item" href="#" onClick={() => handleClick(idioma.id)}>{idioma.idioma}</button></li>
                                ))
                            }
                        </ul>
                    </li>
                </ul>
                <div className="navbar-user">
                    {user ? (
                        <>
                            <AccountCircleIcon className="user-icon" />
                            <span className="user-name">{user.username}</span>
                            <button className="logout-button" onClick={handleLogout}>{gettext('tagLogout')}</button>
                        </>
                    ) : (
                        <>
                            <Link to="/login" className="auth-link">{gettext('tagLogin')}</Link>
                                <Link to="/registrar" className="auth-link">{gettext('tagRegistro')}</Link>
                        </>
                    )}
                </div>
            </div>
        </nav>
    );
}
