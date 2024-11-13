import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useUsers } from '../hooks/useUsers';
import { usePermissions } from '../hooks/usePermissions';

export const Permisos = () => {
    const navigate = useNavigate();
    const { users } = useUsers();
    const [selectedRole, setSelectedRole] = useState({});
    const [permisos, setPermisos] = useState({});
    const { asignarPermiso } = usePermissions();
    const handleClick = (user) => {
        const rolSeleccionado = rolDictionary[selectedRole[user]];
        asignarPermiso(user, rolSeleccionado);
        navigate("/");
    };
    useEffect(() => {
        fetchPermisos();
    }, []);
    const fetchPermisos = async () => {
        try {
            const response = await fetch("api/permiso/permisos", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                },
            });

            if (response.ok) {
                const data = await response.json();
                setPermisos(data.permisos);
            } else {
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
            }
        } catch (error) {
            console.log("Error during login:", error);
        }

    };

    const handleChange = (userId, role) => {
        setSelectedRole(prevRoles => ({
            ...prevRoles,
            [userId]: role
        }));
    };

    const rolDictionary = {
        "Admin": 6,
        "Master": 8,
        "Cliente": 7
    };

    const getUserPermissions = (userId) => {
        const userPermissions = permisos.find(permiso => permiso.usuarioId === userId);
        return userPermissions || { permisosPadre: "", permisosHijos: "" };
    };

    return (
        <div className="container my-5">
            <h2 className="text-center mb-4">Asignación de Roles</h2>
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th>Nombre</th>
                        <th>Email</th>
                        <th>Rol</th>
                        <th>Permisos</th>
                        <th>Acciones</th>
                        <th>Asignar Rol</th>
                    </tr>
                </thead>
                <tbody>
                    {users && users?.usuarios.map(user => {
                        const { permisosPadre, permisosHijos } = getUserPermissions(user.id);

                        return (
                            <tr key={user.id}>
                                <td>{user.nombre}</td>
                                <td>{user.email}</td>
                                <td>{permisosPadre}</td>
                                <td>{permisosHijos}</td>
                                <td>
                                    <div className="dropdown">
                                        <button
                                            className="btn btn-outline-secondary dropdown-toggle"
                                            type="button"
                                            id={`dropdown-roles-${user.id}`}
                                            data-bs-toggle="dropdown"
                                            aria-expanded="false"
                                        >
                                            {selectedRole[user.id] || "Seleccionar Rol"}
                                        </button>
                                        <ul className="dropdown-menu" aria-labelledby={`dropdown-roles-${user.id}`}>
                                            <li><button className="dropdown-item" onClick={() => handleChange(user.id, 'Admin')} >Admin</button></li>
                                            <li><button className="dropdown-item" onClick={() => handleChange(user.id, 'Master')}>Master</button></li>
                                            <li><button className="dropdown-item" onClick={() => handleChange(user.id, 'Cliente')}>Cliente</button></li>
                                        </ul>
                                    </div>
                                </td>
                                <td>
                                    <button
                                        className="btn btn-primary"
                                        onClick={() => handleClick(user.id)}
                                    >
                                        Asignar Rol
                                    </button>
                                </td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );
};
