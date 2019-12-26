$(function () {

    $('#btnGuardar').on('click', function (e) {
        e.preventDefault();

        var data = {};
        var Errors = [];
        var div = $(this).closest('.jumbotron').attr('Id');
        var controles = $('#' + div + ' :input[type=text]');

        $.each(controles, function (i, elemento) {
            var item = $(elemento).val().eliminaEspacios();
            if (item != '') {
                var ident = $('#btnGuardar').attr('data-cve');
                if (ident != '') {
                    data['scCoId'] = parseInt(ident);
                }
                
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
                url: window.rootUrl + "Cobertura/RegistraCobertura",
                data: { jsonTransaccion: data },
                success: function (res) {
                    if (res.mensaje == "OK") {
                        alert("Operación realizada con éxito.");
                        location.reload();
                    }
                    else {
                        alert(res.respuesta);
                    }
                }
            });
        }
    });

    $('input, select').focus(function (e) {
        e.preventDefault();

        if ($(e.target).hasClass('hasError')) {
            $(e.target).removeClass('hasError');
        }
    });

    $("#tblConsulta tbody").on('click', '.update', function (e) {
        e.preventDefault();
        $(this).closest('tr').find('td').each(function (column, td) {
            var item = $(td).html();
            var itemName = $(td).attr('name');

            if (itemName == 'scCoId') {
                $('#btnGuardar').attr('data-cve', item);
            }
            else {
                $('input[name="' + itemName + '"]').val(item);
            }
        });
        $('#divConsulta').hide();
        $('#divCobertura').show();
        $('#btnNuevo').css('display', 'none');
    });

    $("#tblConsulta tbody").on('click', '.delete', function (e) {
        e.preventDefault();
        $(this).closest('tr').find('td').each(function (column, td) {
            var item = $(td).html();
            var itemName = $(td).attr('name');

            if (itemName == 'scCoId') {

                $.ajax({
                    type: 'POST',
                    url: window.rootUrl + "Cobertura/EliminaCobertura",
                    data: { cobertura: parseInt(item) },
                    success: function (res) {
                        if (res.mensaje == "OK") {
                            alert("Operación realizada con éxito.");
                            location.reload();
                        }
                        else {
                            alert(res.respuesta);
                        }
                    }
                });
            }
        });
    });

    $("#btnNuevo").on('click', function (e) {
        e.preventDefault();
        $('#txtNombre').val('');
        $('#btnGuardar').attr('data-cve', '');
        $('#divConsulta').hide();
        $('#btnNuevo').css('display', 'none');
        $('#divCobertura').show();
    });

    $("#btnSalir").on('click', function (e) {
        e.preventDefault();
        $('#txtNombre').val('');
        $('#btnGuardar').attr('data-cve', '');
        $('#divConsulta').show();
        $('#divCobertura').hide();
        $('#btnNuevo').css('display', '');
    });

    $("#lnkCerrar").on('click', function (e) {
        e.preventDefault();
        window.location.href = window.rootUrl + 'Home/Index';
    });

    $(".nav-link").on('click', function (e) {
        e.preventDefault();
        window.location.href = window.rootUrl + $(this).attr('id');
    });
});

String.prototype.eliminaEspacios = function () {
    return this.replace(/\s+/g, ' ').trim();
};
