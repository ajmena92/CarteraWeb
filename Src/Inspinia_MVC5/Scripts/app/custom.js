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

    /*Dropdown Menu Main*/
    $('.dropdown_main').click(function () {
        $(this).attr('tabindex', 1).focus();
    $(this).toggleClass('active');
    $(this).find('.dropdown_main-menu').slideToggle(300);
});
        $('.dropdown_main').focusout(function () {
        $(this).removeClass('active');
    $(this).find('.dropdown_main-menu').slideUp(300);
});
        //$('.dropdown_main .dropdown_main-menu li').click(function () {
        //    $(this).parents('.dropdown_main').find('span').text($(this).text());
        //    $(this).parents('.dropdown_main').find('input').attr('value', $(this).attr('id'));
        //});
        /*End Dropdown Menu*/


        //$('.dropdown_main-menu li').click(function () {
        //    var input = '<strong>' + $(this).parents('.dropdown_main').find('input').val() + '</strong>',
        //        msg = '<span class="msg">Hidden input value: ';
        //    $('.msg').html(msg + input + '</span>');
        //});