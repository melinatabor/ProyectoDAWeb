import React from 'react';
import { Producto } from '../components/Producto';
import tapas from '../assets/tapas.jpg';
import das from '../assets/das.jpg';
import calsa from '../assets/calsa.jpg';
import panpapa from '../assets/pan-papa.jpg';
import panqueso from '../assets/pan-queso.jpg';
import pansesamo from '../assets/pan-sesamo.jpg';
import panmulti from '../assets/pan-multicereal.jpg';
import pansalva from '../assets/salvado.jpg';
import '../styles/productos.css';


export const Productos = ({ addToCart }) => {
    const products = [
        { id: 1, name: 'Tapas de Empanada', image: tapas, price: 650, description: '12 Tapas hojaldradas caseras Don Alfonso.' },
        { id: 2, name: 'Tapas Rotiseras', image: das, price: 3500, description: 'Tubo de 4 docenas de tapas caseras Don Alfonso.' },
        { id: 3, name: 'Levadura Calsa', image: calsa, price: 400, description: 'Levadura marca Calsa.' },
        { id: 4, name: 'Pan de Papa', image: panpapa, price: 1500, description: 'Pan de papa para hamburguesas por 4 unidades.' },
        { id: 5, name: 'Pan de Queso', image: panqueso, price: 1500, description: 'Pan de queso para hamburguesas por 4 unidades.' },
        { id: 6, name: 'Pan de Sesamo', image: pansesamo, price: 1500, description: 'Pan de sesamo para hamburguesas por 4 unidades.' },
        { id: 7, name: 'Pan Lactal Multicereal', image: panmulti, price: 1600, description: 'Pan lactal multicereal pan & pan.' },
        { id: 8, name: 'Pan Lactal de Salvado', image: pansalva, price: 1300, description: 'Pan lactal salvado pan & pan.' },

    ];

    return (
        <div className="products-page">
            <h2>Nuestros Productos</h2>
            <div className="products-list">
                {products.map((product) => (
                    <Producto key={product.id} product={product} onAddToCart={addToCart} />
                ))}
            </div>
        </div>
    );
}