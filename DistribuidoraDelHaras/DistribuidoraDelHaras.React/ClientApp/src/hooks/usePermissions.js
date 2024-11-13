import { useState } from "react";

export const usePermissions = (user) => {
    const [permissions, setPermissions] = useState(null);

    const fetchPermissions = async () => {
        try {
            const response = await fetch(`api/permiso/permisos-usuario/${user}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                },
            });

            if (response.ok) {
                const data = await response.json();
                setPermissions(data.permisos);
            } else {
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
            }
        } catch (error) {
            console.error("Error al cargar los permisos:", error);
        }
    };

    const asignarPermiso = async (user, permiso) => {
        try {
            const response = await fetch(`api/permiso/asignar-permiso/${user}/${permiso}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ permiso: permiso })
            });

            if (response.ok) {
                const data = await response.json();
                let msg = data.message;
                alert(msg);
            } else {
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
            }
        } catch (error) {
            console.error("Error al asignar el permiso:", error);
        }
    }
 

    return { permissions, fetchPermissions, asignarPermiso };
}