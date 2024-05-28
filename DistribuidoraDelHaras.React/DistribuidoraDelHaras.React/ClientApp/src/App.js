import "bootstrap/dist/css/bootstrap.min.css"
import { useEffect, useState } from "react";

const App = () => {

    const [usuarios, setUsuarios] = useState([]);
    const [cargando, setCargando] = useState(true);

    const mostrarUsuarios = async () => {
        try {
            const response = await fetch("api/usuario");

            if (response.ok) {
                const data = await response.json();
                console.log(data);
                setUsuarios(data);
            } else
                console.log("Error: " + response.statusText);

        } catch (error) {
            console.log("Error fetching data:", error);
        } finally {
            setCargando(false);
        }
    }

    useEffect(() => {
        mostrarUsuarios();
    }, []);

    return (
        <div className="container mt-5">
            <h1><strong>Lista de Usuarios</strong></h1>
            {cargando ? (
                <div className="d-flex justify-content-center align-items-center" style={{ height: "50vh" }}>
                    <div className="spinner-border text-primary" role="status" style={{ width: "3rem", height: "3rem" }}>
                        <span className="visually-hidden">Cargando...</span>
                    </div>
                </div>
            ) : (
                <table className="table">
                    <thead>
                        <tr>
                            <th>Nombre</th>
                            <th>Apellido</th>
                            <th>Username</th>
                            <th>Email</th>
                            <th>Password</th>
                        </tr>
                    </thead>
                    <tbody>
                        {usuarios.map((u, index) => (
                            <tr key={u.id || index}>
                                <td>{u.nombre}</td>
                                <td>{u.apellido}</td>
                                <td>{u.username}</td>
                                <td>{u.email}</td>
                                <td>{u.password}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}

export default App;