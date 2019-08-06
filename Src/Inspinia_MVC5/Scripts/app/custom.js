//Obtiene la fecha y la actualiza
var ano = (new Date).getFullYear();
$(document).ready(function () {
    $('.year').text(ano);
    $(".animated").removeClass("d-none");
    $(".animated").addClass("fadeIn");
});
//Activa Rnago datepicker
$('#data_5 .input-daterange').datepicker({
    keyboardNavigation: false,
    forceParse: false,
    autoclose: true
});  