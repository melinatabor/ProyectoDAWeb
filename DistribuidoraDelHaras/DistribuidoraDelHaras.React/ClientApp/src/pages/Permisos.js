import React, { useState } from 'react';
import { useUsers } from '../hooks/useUsers';
import { usePermissions } from '../hooks/usePermissions';

export const Permisos = () => {
    const { users } = useUsers();
    const [selectedRole, setSelectedRole] = useState({});
    const { asignarPermiso } = usePermissions();
    const handleClick = (user) => {
        const rolSeleccionado = rolDictionary[selectedRole[user]];
        asignarPermiso(user, rolSeleccionado);
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

    return (
        <div className="container my-5">
            <h2 className="text-center mb-4">Asignación de Roles</h2>
            <ul className="list-group">
                {users && users?.usuarios.map(user => (
                    <li key={user.id} className="list-group-item d-flex justify-content-between align-items-center">
                        <div>
                            <strong>{user.nombre}</strong> <span className="text-muted">({user.email})</span>
                        </div>
                        <div className="d-flex align-items-center">
                            <div className="dropdown me-2">
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
                            <button
                                className="btn btn-primary"
                                onClick={() => handleClick(user.id)}
                            >
                                Asignar Rol
                            </button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};
