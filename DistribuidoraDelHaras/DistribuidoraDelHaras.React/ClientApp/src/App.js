import React, { useEffect, useState } from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login';
import { Registro } from './pages/Registro';
import { Bitacora } from './pages/Bitacora';
import { Navbar } from './components/Navbar';
import { Productos } from './pages/Productos';
import { Carrito } from './pages/Carrito';
import { useTranslations } from './hooks/useTranslations';
import { Permisos } from './pages/Permisos';


const App = () => {
    const [cart, setCart] = useState([]);
    const addToCart = (product) => {
        setCart((prevCart) => [...prevCart, product]);
    };
    const { setLanguage, getLanguage } = useTranslations();

    const removeFromCart = (productToRemove) => {
        setCart((prevCart) => prevCart.filter(product => product.id !== productToRemove.id));
    };

    useEffect(() => {
        if (!getLanguage()?.idioma) setLanguage(1);
    }, []);


    return (
        <div className="container mt-5">
            <Navbar />
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/registrar" element={<Registro />} />
                <Route path="/bitacora" element={<Bitacora />} />
                <Route path="/productos" element={<Productos addToCart={addToCart} />} />
                <Route path="/carrito" element={<Carrito cart={cart} removeFromCart={removeFromCart} />} />
                <Route path="/permisos" element={<Permisos />} />
            </Routes>
        </div>
    );
}

export default App;