import React, { useContext } from 'react';
import { Card, CardContent, CardMedia, Typography, CardActions, Button, Grid, Container } from '@mui/material';
import { UserContext } from '../components/UserProvider';
import { useNavigate } from 'react-router-dom';

export const Carrito = ({ cart, removeFromCart }) => {
    const { isAuthenticated } = useContext(UserContext);
    const navigate = useNavigate();

    if (!isAuthenticated) navigate('/login');
    if (!cart || cart.length === 0) {
        return (
            <Container className="cart">
                <Typography variant="h4" component="h2" align="center" gutterBottom className="cart-empty-title">
                    El carrito está vacío 🛒
                </Typography>
            </Container>
        );
    }

    const total = cart.reduce((acc, product) => acc + product.price, 0);

    return (
        <Container className="cart">
            <Typography variant="h4" component="h2" align="center" gutterBottom>
                Carrito de Compras
            </Typography>
            <Grid container spacing={3} className="cart-items">
                {cart.map((product) => (
                    <Grid item xs={12} sm={6} md={4} lg={3} key={product.id}>
                        <Card className="cart-item">
                            <CardMedia
                                component="img"
                                height="140"
                                image={product.image}
                                alt={product.name}
                            />
                            <CardContent>
                                <Typography variant="h5" component="div">
                                    {product.name}
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    {product.description}
                                </Typography>
                                <Typography variant="h6" component="div" className="product-price">
                                    ${product.price}
                                </Typography>
                            </CardContent>
                            <CardActions>
                                <Button size="small" color="secondary" onClick={() => removeFromCart(product)}>
                                    Eliminar
                                </Button>
                            </CardActions>
                        </Card>
                    </Grid>
                ))}
            </Grid>
            <div className="cart-total-container">
                <Typography variant="h6" component="div" className="cart-total">
                    Total: ${total}
                </Typography>
                <Button variant="contained" color="primary" className="cart-checkout-button">
                    Proceder al Pago
                </Button>
            </div>
        </Container>
    );
}

