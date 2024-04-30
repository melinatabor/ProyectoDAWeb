function inicio() {
    let e = document.getElementById("lienzo");
    if (e) {
        let lienzo = e.getContext("2d");
        if (lienzo) {
            lienzo.beginPath();
            lienzo.moveTo(100,100);
            lienzo.moveTo(200,200);
            lienzo.moveTo(100,200);
            lienzo.fill();
        }
    } else {
        console.log('El elemento canvas no fue encontrado');
    }
}

window.addEventListener("load",inicio);