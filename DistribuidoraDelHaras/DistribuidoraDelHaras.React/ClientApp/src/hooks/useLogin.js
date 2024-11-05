import { useState, useEffect } from 'react';

export const useLogin = () => {
    const [user, setUser] = useState(() => {
        const storedUser = localStorage.getItem('user');
        return storedUser ? JSON.parse(storedUser) : null;
    });
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const checkUserSession = async () => {
            try {
                const response = await fetch('/api/session/session');
                if (response.ok) {
                    const data = await response.json();
                    if (data.IsAuthenticated) {
                        setUser(data.Username);
                        setIsAuthenticated(true);
                        localStorage.setItem('user', JSON.stringify(data.Username));
                    } else {
                        setUser(null);
                        setIsAuthenticated(false);
                        localStorage.removeItem('user');
                    }
                }
            } catch (error) {
                console.error('Error al verificar la sesión:', error);
                setUser(null);
                setIsAuthenticated(false);
                localStorage.removeItem('user');
            }
        };

        // Ejecutar la verificación solo si no hay un usuario cargado en el estado
        if (!user) {
            checkUserSession();
        }
    }, [user]);

    const loginUser = (username) => {
        setUser(username);
        setIsAuthenticated(true);
        localStorage.setItem('user', JSON.stringify(username));
    };

    const logoutUser = () => {
        setUser(null);
        setIsAuthenticated(false);
        localStorage.removeItem('user');
    };

    const getUserFromStorage = () => {
        const storedUser = localStorage.getItem('user');
        return storedUser ? JSON.parse(storedUser) : null;
    };

    return {
        user,
        isAuthenticated,
        loginUser,
        logoutUser,
        getUserFromStorage
    };
};
