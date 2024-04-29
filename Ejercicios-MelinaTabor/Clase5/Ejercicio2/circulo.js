function iniciar() {
    var elemento = document.getElementById('lienzol');
    var lienzol = elemento.getContext('2d');

    lienzol.beginPath();
    lienzol.arc(100, 100, 50, 0, Math.PI*2, false);
    lienzol.stroke();

    lienzol.beginPath();
    lienzol.arc(200, 150, 30, 0, Math.PI*2, false);
    lienzol.stroke();

    lienzol.beginPath();
    lienzol.arc(300, 200, 20, 0, Math.PI*2, false);
    lienzol.stroke();
}

window.addEventListener('load', iniciar, false);
