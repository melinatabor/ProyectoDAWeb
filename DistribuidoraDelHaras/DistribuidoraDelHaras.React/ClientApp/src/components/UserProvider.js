import React, { createContext, useState, useEffect } from 'react';

export const UserContext = createContext();

export const UserProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const checkUserSession = async () => {
            try {
                const response = await fetch('/api/session/session');
                const data = await response.json();
                console.log(data);
                setIsAuthenticated(data.IsAuthenticated);
                setUser(data.IsAuthenticated ? data.Username : null);
            } catch (error) {
                console.error('Error al verificar la sesión:', error);
            }
        };

        checkUserSession();
    }, []);

    const updateUser = (newUser) => {
        setUser(newUser);
        setIsAuthenticated(newUser !== null);
    };

    return (
        <UserContext.Provider value={{ user, updateUser, isAuthenticated }}>
            {children}
        </UserContext.Provider>
    );
};
