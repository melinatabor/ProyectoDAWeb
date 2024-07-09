import React from 'react';
import { Card, CardContent, CardMedia, Typography, Button } from '@mui/material';
import '../styles/producto.css';

export const Producto = ({ product, onAddToCart }) => {
    return (
        <Card className="product-card">
            <CardMedia
                component="img"
                alt={product.name}
                height="200"
                image={product.image}
            />
            <CardContent className="product-content">
                <Typography variant="h5" component="div" className="product-name">
                    {product.name}
                </Typography>
                <Typography variant="body2" color="text.secondary" className="product-description">
                    {product.description}
                </Typography>
                <Typography variant="h6" component="div" className="product-price">
                    ${product.price}
                </Typography>
                <Button
                    variant="contained"
                    className="add-to-cart-button"
                    onClick={() => onAddToCart(product)}
                >
                    Agregar al Carrito
                </Button>
            </CardContent>
        </Card>
    );
};
