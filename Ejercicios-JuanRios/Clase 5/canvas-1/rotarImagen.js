function retornarLienzo(id){
    let canvas = document.getElementById(id);
    return canvas.getContext ? canvas.getContext("2d") : false;
}

var avance = 0;
var img1;

function dibujar() {
    let lienzo = retornarLienzo("lienzo");
    
    if (lienzo) {
        lienzo.clearRect(0, 0, 600, 600);
        lienzo.save();
        lienzo.translate(300,300);
        lienzo.rotate(avance);
        lienzo.drawImage(img1,-125,-125);
        avance+=0.05;
        if (avance > Math.PI*2)
            avance=0;
        lienzo.restore();
    }
}

function inicio()
{
    img1 = new Image();
    img1.src = "image.png";
    img1.onload = function() {
        setInterval(dibujar,50)
    };
}