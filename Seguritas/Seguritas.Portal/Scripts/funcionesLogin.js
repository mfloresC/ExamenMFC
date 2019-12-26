$(function () {

    $('#lnkNuevoRegistro, #btnSalir').on('click', function (e) {
        e.preventDefault();
        var pageHide = $(this).closest('.jumbotron').attr('Id');
        var pageShow = $(this).closest('.jumbotron').next().attr('Id') == undefined ? $(this).closest('.jumbotron').prev().attr('Id') : $(this).closest('.jumbotron').next().attr('Id');
        $('#' + pageHide).hide();
        $('#' + pageShow).show();
    });

    $('#btnCrear, #btnLogin').on('click', function (e) {
        e.preventDefault();

        var data = {};
        var metodo = '';
        var Errors = [];
        var controles = [];
        var div = $(this).closest('.jumbotron').attr('Id');
        if (div == 'divRegistro') {
            controles = $('#' + div + ' :input[type=text]');
        }
        else
        {
            controles = $('#' + div + ' :input[type=text], :input[type=password]');
        }
        

        metodo = controles.length > 2 ? 'RegistraUsuario' : 'FirmarUsuario'

        $.each(controles, function (i, elemento) {
            var item = $(elemento).val().eliminaEspacios();
            if (item != '') {
                data[$(elemento).attr('name')] = item;
            }
            else {
                Errors.push($(elemento).attr('id'));
            }
        });

        if (Errors.length > 0) {
            $.each(Errors, function (i, elemento) {
                $('#' + elemento).addClass('hasError');
            });
        }
        else {

            data = JSON.stringify(data);
            $.ajax({
                type: 'POST',
                url: window.rootUrl + "Home/" + metodo,
                cache: false,
                data: { jsonTransaccion: data },
                success: function (res) {
                    if (res.mensaje == "OK") {
                        alert("Operación realizada con éxito.");
                        $('#btnSalir').click();
                    }
                    else if (res.mensaje == "Allow") {
                        window.location.href = window.rootUrl + 'Cliente/Page';
                    }
                    else {
                        alert(res.respuesta);
                    }
                }
            });
        }
    });

    $('input').focus(function (e) {
        e.preventDefault();

        if ($(e.target).hasClass('hasError')) {
            $(e.target).removeClass('hasError');
        }
    });
});

String.prototype.eliminaEspacios = function () {
    return this.replace(/\s+/g, ' ').trim();
};