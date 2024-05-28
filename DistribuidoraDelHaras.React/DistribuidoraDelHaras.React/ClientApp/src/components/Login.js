import "bootstrap/dist/css/bootstrap.min.css";
import { useState } from "react";

const Login = () => {
    const [username, setUsername] = useState("");
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
                let msg = JSON.stringify(data);
                alert(msg);
                console.log(msg);
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
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                </div>
                <div className="form-group mb-3">
                    <label htmlFor="password">Password</label>
                    <input
                        type="password"
                        className="form-control"
                        id="password"
                        value={password}
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
