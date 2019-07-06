$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {                        
        var bindingformimg = false;
        //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        if ($(this).hasClass('form_img')) {
            bindingformimg = true;
        }
        $('#myModalContent').load(this.href, function () {           
            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            if (bindingformimg)
            {
                bindFormImage(this);
            }
            else {
                bindForm(this);
            }
        });        
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {       
        var formData = new FormData($(this)[0]);
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(), 
            cache: false,        
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    if (result.update == null) {
                        $('#replacetarget').load(result.url); //  Load data from the server and place the returned HTML into the matched element
                    }
                    else
                    {
                         // $(result.update).load(result.url); Load data from the server and place the returned HTML into the matched element
                        window.location.href = result.url;
                    }                    
                    $('#myModalContent').empty();
                } else {
                    $('#myModalContent').empty();
                    $('#myModalContent').html(result);
                    bindForm(dialog);
                }           
            },
            error: function (err) {
                console.error(err);
            }         
        });
        return false;
    });
}

function bindFormImage(dialog) {
    $('form', dialog).submit(function () {
        
        var formData = new FormData($(this)[0]);
        $.ajax({
            //type: 'POST',
            url: this.action,
            type: this.method,
            mimeType: 'application/json',
            dataType: 'json',
            data: formData,
            contentType: false,
            processData: false,
            enctype: 'multipart/form-data',
            cache: false,
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    $('#replacetarget').load(result.url); //  Load data from the server and place the returned HTML into the matched element
                    $('#myModalContent').empty();
                } else {
                    $('#myModalContent').empty();
                    $('#myModalContent').html(result);
                    bindForm(dialog);
                }
                endWait();
            },
            error: function (err) {
                endWait();
            }         
        });
        return false;
    });
}
