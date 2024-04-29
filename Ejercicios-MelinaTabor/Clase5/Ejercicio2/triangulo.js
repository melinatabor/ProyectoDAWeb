function iniciar() {
    var elemento = document.getElementById('lienzo');
    var lienzo = elemento.getContext('2d');
    
    lienzo.fillStyle = 'red';
    lienzo.beginPath();
    lienzo.moveTo(100, 100);
    lienzo.lineTo(200, 200);
    lienzo.lineTo(100, 200);
    lienzo.fill();

    lienzo.fillStyle = 'green';
    lienzo.beginPath();
    lienzo.moveTo(250, 100);
    lienzo.lineTo(350, 200);
    lienzo.lineTo(250, 200);
    lienzo.fill();

    lienzo.fillStyle = 'blue';
    lienzo.beginPath();
    lienzo.moveTo(400, 100);
    lienzo.lineTo(500, 200);
    lienzo.lineTo(400, 200);
    lienzo.fill();
}
addEventListener("load", iniciar);