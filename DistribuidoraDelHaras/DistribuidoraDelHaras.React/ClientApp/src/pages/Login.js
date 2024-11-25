import React, { useEffect, useState, useRef } from "react";
import { useSession } from "../hooks/useSession";
import { useNavigate } from "react-router-dom";


const Login = () => {
    const navigate = useNavigate();
    const { set } = useSession();
    const [username, setUser] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const [showModal, setShowModal] = useState(false);
    const [loginSuccessful, setLoginSuccessful] = useState(false);
    const [userType, setUserType] = useState("");
    const [modificaciones, setModificaciones] = useState([]);

    const handleLogin = async () => {
        const loginData = { username, password };
        try {
            const response = await fetch("api/login/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(loginData)
            });

            if (response.ok) {
                const data = await response.json();
                setLoginSuccessful(true);
                set(data);
                
            } else {
                const errorData = await response.json();
                const array = errorData.message.split(',');
                const first = array.shift();
                setUserType(first);
                setModificaciones(array);
                setErrorMessage(errorData.message === "2" ? "Ocurrió un error inesperado. Por el momento no tiene acceso al sistema." : errorData.message);
                setShowModal(true);
                setLoginSuccessful(false);
            }
        } catch (error) {
            setErrorMessage("Ocurrió un error inesperado. Intente nuevamente.");
            setShowModal(true);
            setLoginSuccessful(false);
        }

    };

    useEffect(() => {
        if (loginSuccessful) {
            navigate("/");
            window.location.reload();
        }
    }, [loginSuccessful]);


    const closeModal = () => {
        setShowModal(false);
    };

    const handleRecalcularDV = async () => {
        try {
            const response = await fetch("/api/recalculardv", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (response.ok) {
                const data = await response.json();

                alert(data.message);
            } else {
                const errorData = await response.json();
                alert(errorData.message);
            }
        } catch (error) {
            console.log("Error al restaurar la base de datos:", error);
        }
    }


    const fileInputRef = useRef(null);

    const handleButtonClick = () => {
        fileInputRef.current.click();
    };

    const handleFileChange = async (event) => {
        try {
            const response = await fetch("/api/recalculardv", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                }
            });

            if (response.ok) {

                const data = await response.json();

                alert("Se ha restaurado el backup correctamente.");
            } else {
                const errorData = await response.json();
                alert(errorData.message);
            }
        } catch (error) {
            console.log("Error al restaurar la base de datos:", error);
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center vh-100">
            <div className="card p-4" style={{ width: "300px" }}>
                <h3 className="text-center mb-4">Login</h3>
                <div className="form-group mb-3">
                    <label htmlFor="username">Username</label>
                    <input
                        type="text"
                        className="form-control"
                        id="username"
                        onChange={(e) => setUser(e.target.value)}
                    />
                </div>
                <div className="form-group mb-3">
                    <label htmlFor="password">Password</label>
                    <input
                        type="password"
                        className="form-control"
                        id="password"
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </div>
                <button
                    type="button"
                    className="btn btn-primary w-100"
                    onClick={handleLogin}
                >
                    Login
                </button>
            </div>

            {showModal && (
                <div
                    className="modal d-block"
                    style={{
                        backgroundColor: "rgba(0, 0, 0, 0.5)",
                        display: "flex",
                        justifyContent: "center",
                        alignItems: "center",
                    }}
                >
                    <div className="modal-dialog">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title text-danger">
                                    ¡Advertencia!
                                </h5>
                                <button
                                    type="button"
                                    className="btn-close"
                                    onClick={closeModal}
                                ></button>
                            </div>
                            <div className="modal-body">
                                <p>{errorMessage}</p>
                                {modificaciones.map(registro => (
                                    <p>{registro}</p>
                                ))}
                                {userType === "1" &&
                                    <>
                                    <p>Seleccione una opción:</p>
                                    <button className="btn btn-primary me-2" onClick={() => handleRecalcularDV()}>
                                        Recomponer el dígito verificador
                                    </button>
                                    <div>
                                        <button className="btn btn-secondary" onClick={() => handleButtonClick()}>Restaurar desde un backup</button>
                                        <input
                                            type="file"
                                            ref={fileInputRef}
                                            style={{ display: 'none' }}
                                            onChange={() => handleFileChange()}
                                        />
                                    </div>
                                </>
                                }
                                
                            </div>
                            <div className="modal-footer">
                                <button
                                    className="btn btn-danger"
                                    onClick={closeModal}
                                >
                                    Salir
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Login;
