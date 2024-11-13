import { useEffect } from "react";
import { useState } from "react";

export const useUsers = () => {
    const [users, setUsers] = useState(null);

    const fetchUsuarios = async () => {
        try {
            const response = await fetch(`api/login/usuarios`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                },
            });

            if (response.ok) {
                const data = await response.json();
                setUsers(data);
            } else {
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
            }
        } catch (error) {
            console.error("Error al cargar los permisos:", error);
        }
    }

    useEffect(() => {
        setTimeout(() => {
            fetchUsuarios();
        }, 200);
    }, []);

    return { users, fetchUsuarios };
}