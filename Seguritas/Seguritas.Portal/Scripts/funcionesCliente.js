$(function () {

    $('#btnGuardar').on('click', function (e) {
        e.preventDefault();

        var data = {};
        var Errors = [];
        var sinRows = false;
        var div = $(this).closest('.jumbotron').attr('Id');
        var controles = $('#' + div + ' :input[type=text], select option:selected');

        $.each(controles, function (i, elemento) {
            var item = $(elemento).val().eliminaEspacios();
            if (item != '') {
                var ident = $('#btnGuardar').attr('data-cve');
                if (ident != '') {
                    data['scCId'] = parseInt(ident);
                }
                if (elemento.nodeName != 'INPUT') {
                    var lstPlan = new Array();
                    $("#tblPlan tbody").find('tr').each(function (column, td) {
                        var elemento = parseInt($(td).attr('id'));
                        lstPlan.push(elemento);
                    });

                    var newlstPlan = lstPlan.filter(function (elem, index, self) {
                        return index === self.indexOf(elem);
                    });

                    if (newlstPlan.length > 0) {
                        data.claveP = newlstPlan;
                    }
                    else {
                        sinRows = true;
                    }
                }
                else {
                    data[$(elemento).attr('name')] = item;
                }
            }
            else {
                Errors.push($(elemento).attr('id'));
            }
        });

        if (Errors.length > 0 || sinRows) {
            $.each(Errors, function (i, elemento) {
                $('#' + elemento).addClass('hasError');
            });

            if (sinRows) {
                alert("Debes de agregar por lo menos un plan.");
            }
        }
        else {
            data = JSON.stringify(data);
            $.ajax({
                type: 'POST',
                url: window.rootUrl + "Cliente/RegistraCliente",
                data: { jsonTransaccion: data },
                success: function (res) {
                    if (res.mensaje == "OK")
                    {
                        alert("Operación realizada con éxito.");
                        location.reload();
                    }
                    else
                    {
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

    //$("#tblConsulta tbody").on('click', '.update', function (e) {
    //    e.preventDefault();
    //    $(this).closest('tr').find('td').each(function (column, td) {
    //        var item = $(td).html();
    //        var itemName = $(td).attr('name');

    //        if (itemName == 'scCId') {
    //            $('#btnGuardar').attr('data-cve', item);
    //        }
    //        else if (itemName == 'nombreP') {

    //        }
    //        else {
    //            $('input[name="' + itemName + '"]').val(item);
    //        }
    //    });
    //    $('#divConsulta').hide();
    //    $('#divMaterias').show();
    //    $('#btnNuevo').css('display', 'none');
    //});

    $("#tblConsulta tbody").on('click', '.delete', function (e) {
        e.preventDefault();
        $(this).closest('tr').find('td').each(function (column, td) {
            var item = $(td).html();
            var itemName = $(td).attr('name');

            if (itemName == 'scCId') {

                $.ajax({
                    type: 'POST',
                    url: window.rootUrl + "Cliente/EliminaCliente",
                    data: { cliente: parseInt(item)},
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
        $('#divCliente').show();
    });

    $("#btnSalir").on('click', function (e) {
        e.preventDefault();
        $('#txtNombre').val('');
        $('#btnGuardar').attr('data-cve', '');
        $('#divConsulta').show();
        $('#divCliente').hide();
        $('#btnNuevo').css('display', '');
    });

    $("#lnkCerrar").on('click', function (e) {
        e.preventDefault();
        window.location.href = window.rootUrl + 'Home/Index';
    });

    $("#btnAdd").on('click', function (e) {
        e.preventDefault();
        var nombre = $('#cbPaquete option:selected').val();
        var cve = $('#cbPaquete option:selected').attr('id');
        if (nombre != '')
        {
            $('#tblPlan').show();
            var contenido = '<tr id="' + cve + '">' + '<td name="scCId">' + cve + '</td>' + '<td name="scCNombre">' + nombre + '</td>' + '<td><div class="form-group text-center"><button class="btn lineAjust deleteR">Borrar</button></div>' + '</tr>';
            $("#tblPlan tbody").append(contenido);
        }
    });

    $("#tblPlan tbody").on('click', '.deleteR', function (e) {
        e.preventDefault();
        $(this).closest('tr').remove();
    });

    $(".nav-link").on('click', function (e) {
        e.preventDefault();
        window.location.href = window.rootUrl + $(this).attr('id');
    });
});

String.prototype.eliminaEspacios = function () {
    return this.replace(/\s+/g, ' ').trim();
};
