import React, { createContext, useState } from 'react';

export const UserContext = createContext();

export const UserProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

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
