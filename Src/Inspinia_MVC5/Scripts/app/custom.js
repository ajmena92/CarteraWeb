//Obtiene la fecha y la actualiza
var ano = (new Date).getFullYear();
$(document).ready(function () {   
    $(".animated").removeClass("d-none");
    $(".animated").addClass("fadeIn");
    $('.year').text(ano);
});
