//Obtiene la fecha y la actualiza
var ano = (new Date).getFullYear();
$(document).ready(function () {
    $('.year').text(ano);;
});
//Activa Rnago datepicker
$('#data_5 .input-daterange').datepicker({
    keyboardNavigation: false,
    forceParse: false,
    autoclose: true
});  
//Date Picker

//$(document).ready(function () {
//    $('.date').datepicker({
//        changeMonth: true,
//        changeYear: true,
//        dateFormat: "dd/mm/yy"
//    });
//    jQuery.validator.methods.date = function (value, element) {
//        var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
//        if (isChrome) {
//            var d = new Date();
//            return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
//        }
//        else {
//            return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
//        }
//    };
//});
