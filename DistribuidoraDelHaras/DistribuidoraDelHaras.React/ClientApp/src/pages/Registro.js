import React, { useState } from "react";
import {
    Container,
    Typography,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Button,
} from "@mui/material";
import "../styles/registro.css";
import { useNavigate } from "react-router-dom";

export const Registro = () => {
    const navigate = useNavigate();
    const [nombre, setNombre] = useState("");
    const [apellido, setApellido] = useState("");
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [rol, setRol] = useState("");

    const handleRegister = async (e) => {
        e.preventDefault();

        const registro = { nombre, apellido, email, username, password, rol };

        try {
            const response = await fetch("/api/login/registrar", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(registro)
            });

            if (response.ok) {
                await response.json();
                navigate("/");
            } else {
                const errorData = await response.json();
                let msg = errorData.message;
                alert(msg);
            }
        } catch (error) {
            console.log("Error during login:", error);
        }
    };

    return (
        <Container maxWidth="sm" className="register-container">
            <Typography variant="h4" className="register-title">
                Registrarse
            </Typography>
            <form onSubmit={handleRegister}>
                <TextField
                    label="Nombre"
                    variant="outlined"
                    className="register-input"
                    value={nombre}
                    onChange={(e) => setNombre(e.target.value)}
                    required
                />
                <TextField
                    label="Apellido"
                    variant="outlined"
                    className="register-input"
                    value={apellido}
                    onChange={(e) => setApellido(e.target.value)}
                    required
                />
                <TextField
                    label="Email"
                    type="email"
                    variant="outlined"
                    className="register-input"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
                <TextField
                    label="Nombre de Usuario"
                    variant="outlined"
                    className="register-input"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required
                />
                <TextField
                    label="Contraseña"
                    type="password"
                    variant="outlined"
                    className="register-input"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <Button type="submit" variant="contained" color="primary" className="register-button">
                    Registrarse
                </Button>
            </form>
        </Container>
    );
};

