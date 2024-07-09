import React, { useState, useEffect } from 'react';
import '../styles/bitacora.css';

export const Bitacora = () => {
    const [logs, setLogs] = useState([]);

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
