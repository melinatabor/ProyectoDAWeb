import React, { useState } from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login';
import { Registro } from './pages/Registro';
import { Bitacora } from './pages/Bitacora';
import { Navbar } from './components/Navbar';
import { UserProvider } from './components/UserProvider';
import { Productos } from './pages/Productos';
import { Carrito } from './pages/Carrito';

const App = () => {
    const [cart, setCart] = useState([]);

    const addToCart = (product) => {
        setCart((prevCart) => [...prevCart, product]);
    };

    const removeFromCart = (productToRemove) => {
        setCart((prevCart) => prevCart.filter(product => product.id !== productToRemove.id));
    };

    return (
        <div className="container mt-5">
            <UserProvider>
                <Navbar />
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/registrar" element={<Registro />} />
                    <Route path="/bitacora" element={<Bitacora />} />
                    <Route path="/productos" element={<Productos addToCart={addToCart} />} />
                    <Route path="/carrito" element={<Carrito cart={cart} removeFromCart={removeFromCart} />} />
                </Routes>
            </UserProvider>
        </div>
    );
}

export default App;