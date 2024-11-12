import React from 'react';
import '../styles/home.css';

import { Container, Grid, Paper } from '@mui/material';
import heroImage from '../assets/pan-sesamo.jpg';
import productImage1 from '../assets/publicidad-2.jpg';
import productImage2 from '../assets/masitasa.jpg';
import productImage3 from '../assets/empanadirtas.jpg';
import productImage4 from '../assets/pascualiana.jpg';
import aboutImage from '../assets/empa.jpg';
import { useTranslations } from '../hooks/useTranslations';

const Home = () => {
    const { gettext } = useTranslations();
    return (
        <div className="home">
            <section className="hero" id="home">
                <img src={heroImage} alt="Distribuidora del Haras" className="hero-image" />
                <div className="hero-text">
                    <h1>{gettext('headerHome')}</h1>
                    <p>Tu mejor opción en productos alimenticios.</p>
                </div>
            </section>

            <Container>
                <section className="products" id="products">
                    <h2>Algunos de nuestros productos...</h2>
                    <Grid container spacing={4}>
                        <Grid item xs={12} md={6}>
                            <Paper className="product-item">
                                <img src={productImage1} alt="Producto 1" className="product-image" />
                            </Paper>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <Paper className="product-item">
                                <img src={productImage2} alt="Producto 2" className="product-image" />
                            </Paper>
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <Paper className="product-item">
                                <img src={productImage3} alt="Producto 3" className="product-image" />
                            </Paper>
                        </Grid><Grid item xs={12} md={6}>
                            <Paper className="product-item">
                                <img src={productImage4} alt="Producto 4" className="product-image" />
                            </Paper>
                        </Grid>
                    </Grid>
                </section>

                <section className="about" id="about" >
                    <h2>Sobre Nosotros</h2>
                    <div className="about-content">
                        <img src={aboutImage} alt="Sobre Nosotros" className="about-image" />
                        <div className="about-text">
                            <p>Distribuidora del Haras es una empresa dedicada a ofrecer los mejores productos alimenticios a nuestros clientes. Nuestra misión es garantizar la calidad y satisfacción en cada entrega.</p>
                        </div>
                    </div>
                </section>

                <section className="contact" id="contact">
                    <h2>Contacto</h2>
                    <p>Para más información, contáctanos en:</p>
                    <p>Email: info@distribuidoradelharas.com</p>
                    <p>Teléfono: (123) 456-7890</p>
                </section>
            </Container>
        </div>
    );
}

export default Home;