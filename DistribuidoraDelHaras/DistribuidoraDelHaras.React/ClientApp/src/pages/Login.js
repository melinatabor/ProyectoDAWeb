import React, { useState } from "react";
import { useLogin } from "../hooks/useLogin";
import { useNavigate } from "react-router-dom";

const Login = () => {
    const navigate = useNavigate();
    const { loginUser } = useLogin();
    const [username, setUser] = useState("");
    const [password, setPassword] = useState("");


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
                console.log(data, 'holia');
                loginUser(data); 
                navigate("/");
            } else {
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
                console.log(msg);
            }
        } catch (error) {
            console.log("Error during login:", error);
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
        </div>
    );
};

export default Login;
