import React, { useState, useEffect } from 'react';
import { useSession } from '../hooks/useSession';

import { useNavigate } from 'react-router-dom';

import '../styles/bitacora.css';
import { useTranslations } from '../hooks/useTranslations';

export const Bitacora = () => {
    const [logs, setLogs] = useState([]);
    const [desde, setDesde] = useState('');
    const [hasta, setHasta] = useState('');
    const [tipo, setTipo] = useState(1);
    const { user } = useSession();
    const navigate = useNavigate();
    const { gettext } = useTranslations();
    useEffect(() => {
        fetchBitacoraData();
    }, []);

    const fetchBitacoraData = async () => {
        try {
            const response = await fetch('/api/bitacora/bitacora', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ desde, hasta, tipo })
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

    if (!user) {
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
            <h2>{gettext('tagBitacora')}</h2>
            <div className="filtros-fecha">
                <label>Desde: </label>
                <input
                    type="date"
                    value={desde}
                    onChange={(e) => setDesde(e.target.value)}
                />
                <label>Hasta: </label>
                <input
                    type="date"
                    value={hasta}
                    onChange={(e) => setHasta(e.target.value)}
                />
                <label>Tipo: </label>
                <select
                    value={tipo}
                    onChange={(e) => setTipo(parseInt(e.target.value))}
                >
                    <option value={1}>INFO</option>
                    <option value={2}>ERROR</option>
                </select>
                <button onClick={fetchBitacoraData}>Filtrar</button>
            </div>
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
