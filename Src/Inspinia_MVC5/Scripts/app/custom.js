//Obtiene la fecha y la actualiza
var ano = (new Date).getFullYear();
$(document).ready(function () {
    $('.year').text(ano);;
});

//Mensaje de Error
function AccesErr() {
    var currentScrollPos = $(window).scrollTop();
    if (currentScrollPos > 40) {
        currentScrollPos -= 40;
    }
    else {
        currentScrollPos = 60;
    }
    $("#msjaccesserror").css({ 'top': currentScrollPos + 'px' });
    $("#msjaccesserror").fadeIn(500).delay(4500).fadeOut(1500);
}