/*slide show trang chur*/
const myslide = document.getElementsByClassName("myslide");
let counter = 0;
setInterval(autoslide, 2000);

function autoslide() {
    counter++;
    slidefun(counter);
}

function plusSlides(n) {
    counter += n;
    slidefun(counter);
    clearInterval(timer);
    timer = setInterval(autoslide, 2000);
}

function currentSlide(n) {
    counter = n;
    slidefun(counter);
    clearInterval(timer);
    timer = setInterval(autoslide, 2000);
}

function slidefun(n) {
    let i;
    for (i = 0; i < myslide.length; i++) {
        myslide[i].style.display = "none";
    }
    if (n >= myslide.length) {
        counter = 0;
        n = 0;
    }
    if (n < 0) {
        counter = myslide.length - 1;
        n = myslide.length - 1;
    }
    if (myslide[n]) {
        myslide[n].style.display = "block";
    }
}
