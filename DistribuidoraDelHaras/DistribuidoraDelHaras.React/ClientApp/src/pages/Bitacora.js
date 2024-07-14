/* eslint-disable react-hooks/rules-of-hooks */
import React, { useState, useEffect, useContext } from 'react';
import { UserContext } from '../components/UserProvider';
import { useNavigate } from 'react-router-dom';

import '../styles/bitacora.css';

export const Bitacora = () => {
    const [logs, setLogs] = useState([]);
    const { isAuthenticated } = useContext(UserContext);
    const navigate = useNavigate();

    useEffect(() => {
            fetchBitacoraData();
    }, []);

    const fetchBitacoraData = async () => {
        try {
            const response = await fetch('/api/bitacora/bitacora', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.ok) {
                const data = await response.json();
                setLogs(data.filas);
            } else {
                const errorData = await response.json();
                console.log(errorData.message);
            }
        } catch (error) {
            console.log('Error al obtener los datos:', error);
        }
    };

    const onClick = () => {
        navigate('/login');
    };

    if (!isAuthenticated) {
        return (
            <div className="bitacora-container">
                <h1>Error</h1>
                <p>Inicia sesión con un usuario de tipo admin para acceder a esta página.</p>
                <button onClick={onClick}>Ir al Login</button>
            </div>
        );
    }

    return (
        <div className="bitacora-container">
            <h2>Bitácora</h2>
            <div className="logs">
                <table>
                    <thead>
                        <tr>
                            <th>Usuario</th>
                            <th>Mensaje</th>
                            <th>Fecha y Hora</th>
                        </tr>
                    </thead>
                    <tbody>
                        {logs.length > 0 ? (
                            logs.map((log) => (
                                <tr key={log.id}>
                                    <td>{log.usuario}</td>
                                    <td>{log.mensaje}</td>
                                    <td>{new Date(log.fecha).toLocaleString()}</td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan="3">No hay entradas en la bitácora.</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
};
