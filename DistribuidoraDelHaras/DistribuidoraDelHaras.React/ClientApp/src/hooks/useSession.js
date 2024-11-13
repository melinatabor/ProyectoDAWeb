import { useMemo } from 'react';

export const useSession = () => {
    const user = useMemo(() => {
        const savedUser = sessionStorage.getItem('user');
        return savedUser ? JSON.parse(savedUser) : null;
    }, []);

    const set = (newUser) => {
        sessionStorage.setItem('user', JSON.stringify(newUser));
    };

    const clear = () => {
        sessionStorage.removeItem('user');
    };

    return {
        user,
        set,
        clear,
    };
};