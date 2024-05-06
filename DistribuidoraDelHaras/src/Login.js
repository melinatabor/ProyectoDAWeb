import React, { useState } from 'react';
import './login.scss';

export const Login = () =>  {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  // TODO vamos a mandar los datos a la BD
  const handleLogin = () => {
    console.log('Username:', username);
    console.log('Password:', password);
    alert(`¡Bienvenido! \nUsername: ${username} \nPassword: ${password}`);
  };

  return (
    <div className="login-container">
      <div className="welcome-message">Bienvenido al sistema de distribuidora del haras</div>
      <h2>Inicia Sesión</h2>
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <button onClick={handleLogin}>Login</button>
    </div>
  );
}
