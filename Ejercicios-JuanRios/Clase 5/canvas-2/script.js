function inicio() {
    let e = document.getElementById("lienzo");
    if (e) {
        let lienzo = e.getContext("2d");
        if (lienzo) {
            lienzo.beginPath();
            lienzo.arc(100, 100, 50, 0, Math.PI * 2, false);
            lienzo.stroke();
        }
    } else {
        console.log('El elemento canvas no fue encontrado');
    }
}

window.addEventListener("load",inicio,false);