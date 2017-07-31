$(document).ready(function () {
    var valueEditModal = false;
    //Ativar sub-mennu
    $('#admin').addClass('active open');
    $('#adminAjustesPonto').addClass('active');

    //Abrir modal para confirmar exclusão

    $('#datatable').on('click', '.btnExcluir', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        $('#mod-danger').data('id', id).modal('show');
    });



    $('#btnConfirmaExclusao').click(function () {
        var id = $('#mod-danger').data('id');

        $.ajax({
            url: '/AjustePonto/Excluir',
            data: { pontoId: id },
            type: "POST",
            cache: false,
            success: function (response) {
                if (response.pontoExcluido) {
                    //Remove a linha visual da lista de usuários
                    var table = $('#datatable').DataTable();
                    table.row($('#datatable').find('#row_' + id)).remove().draw();

                    $('#mod-danger').modal('hide');
                }
            },
            error: function (response) {
                alert('error ao excluir');
            }
        });
    });

    //Remover o item vazio (importante no cadastro de documentos gerais)
    $("#LocalId option[value='1']").remove();

    $('#LocalId').change(function () {
        HabilitarCampos();
        $("#EmpresaId :gt(0)").remove();
        if ($('#LocalId').val() > 0 || $('#LocalId').val() != '') {
            $.getJSON('/AjustePonto/ListaEmpresas', { localId: $('#LocalId').val() }, function (data) {
                //     $('#EmpresaId').append('<option value="0">Todos</option>');
                $.each(data, function (index, item) {
                    $('#EmpresaId').append('<option value=' +
                      item.empresaId + '>' + item.nome + '</option>');
                });
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert('Error getting empresas!');
            });
        }
    });

    $('#EmpresaId').change(function () {
        $("#ContratadoId :gt(0)").remove();
        if ($('#EmpresaId').val() != "") {
            $.getJSON('/AjustePonto/ListaContratados', { empresaId: $('#EmpresaId').val() }, function (data) {
                if ($('#EmpresaId').val() != 0 || $('#EmpresaId').val()) {
                    if (data.length > 0) {
                        $('#ContratadoId').append('<option value="0">Todos</option>');
                        HabilitarCampos();
                    }
                    else {
                        msgAtencao();
                        DesabilitarCampos();
                    }
                    $.each(data, function (index, item) {
                        $('#ContratadoId').append('<option value=' +
                          item.contratadoId + '>' + item.nome + '</option>');
                    });
                }
                else if ($('#EmpresaId').val() == 0) {
                    $('#ContratadoId').remove('<option value="0">Todos</option>');
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert('Error getting contratado!');
            });
        }
    });

    $('#Ano').change(function () {
        $("#Mes :gt(0)").remove();
        if ($('#Ano').val() != "") {
            $.getJSON('/AjustePonto/ListaMeses', { ano: $('#Ano').val() }, function (data) {
                if ($('#Ano').val() != 0 || $('#Ano').val() != "") {
                    $.each(data, function (index, item) {
                        $('#Mes').append('<option value=' +
                          item.mesId + '>' + item.nome + '</option>');
                    });
                }
                else {
                    $('#Mes').append('<option value="0">Todos</option>');
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert('Error getting mêses!');
            });
        }
    });

    $('#Mes').change(function () {
        $("#Dia :gt(0)").remove();
        if ($('#Mes').val() != "") {
            $.getJSON('/AjustePonto/ListaDias', { mes: $('#Mes').val(), ano: $('#Ano').val() }, function (data) {
                if ($('#Mes').val() != 0 || $('#Mes').val() != "") {
                    $('#Dia').append('<option value="0">Todos</option>');
                    $.each(data, function (index, item) {
                        $('#Dia').append('<option value=' +
                          item.diaId + '>' + item.numDia + '</option>');
                    });
                }
                else {
                    $('#Dia').append('<option value="0">Todos</option>');
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert('Error getting mêses!');
            });
        }
    });

    //Ordenar por hora
    var table = $('#datatable').DataTable();
    table.order([1, 'asc']).draw();

    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");

    $('.btnEditar').on('click', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var registro = $(this).data('registro');
        var hora = $(this).data('hora');
        var justificativa = $(this).data('justificativa');

        var ano = $(this).data('ano');
        var mes = $(this).data('mes');
        var dia = $(this).data('dia');
        var contratadoId = $(this).data('contratado');
        var nome = $(this).data('nome');
        var rg = $(this).data('rg');
        var diabuscado = $(this).data('diabuscado');
        $('#mod-edit').data('ano', ano);
        $('#mod-edit').data('mes', mes);
        $('#mod-edit').data('dia', dia);
        $('#mod-edit').data('contratadoId', contratadoId);
        $('#mod-edit').data('nome', nome);
        $('#mod-edit').data('rg', rg);
        $('#mod-edit').data('diabuscado', diabuscado);

        $('select[id="selectRegistro"]').val(registro);
        $('#inputHora').val(hora);
        $('#inputJustificativa').val(justificativa);

        $('#mod-edit').data('id', id).modal('show');
    });

    $('#selectRegistro').change(function () {

        if ($('#selectRegistro').val() == '-- Selecione --') {
            $('#selectRegistro').css('border-color', 'red');

        }
        else {
            $('#selectRegistro').css('border-color', '#bdc0c7');
            $('#selectRegistro').css('color', '#666');

        }
    })

    $('#inputHora').change(function () {
        var result = ValidaHora($('#inputHora').val());

        if (result) {
            $('#inputHora').css('border-color', '#bdc0c7');
            $('label[for="inputHora"]').css('color', '#666');

        }
        else {
            $('#inputHora').css('border-color', '#ea4335');
            $('label[for="inputHora"]').css('color', '#ea4335');

        }
    })


    $('#HoraRegistro').change(function () {
        var result = ValidaHora($('#HoraRegistro').val());

        if (result) {
            $('#HoraRegistro').css('border-color', '#bdc0c7');
            $('label[for="inputHora"]').css('color', '#666');

        }
        else {
            $('#HoraRegistro').css('border-color', '#ea4335');
            $('label[for="inputHora"]').css('color', '#ea4335');

        }
    })

    $('#btnAlterar').click(function () {

        var id = $('#mod-edit').data('id');
        var registro = $('select[id="selectRegistro"]').val();
        var hora = $('#inputHora').val();
        var justificativa = $('#inputJustificativa').val();
        var resultAlterar = ValidaModal();

        if (resultAlterar) {

            $('#btnAlterar').attr("data-dismiss", "modal");
            var currentTR = $(this).closest("tr");

            $.ajax({
                url: '/AjustePonto/AlterarPonto',
                data: { pontoId: id, registro: registro, hora: hora, justificativa: justificativa },
                type: "POST",
                cache: false,
                success: function (response) {
                    if (response.pontoAlterado) {

                        var ano = $('#mod-edit').data('ano');
                        var mes = $('#mod-edit').data('mes');
                        var dia = $('#mod-edit').data('dia');
                        var contratadoId = $('#mod-edit').data('contratadoId');
                        var nome = $('#mod-edit').data('nome');
                        var rg = $('#mod-edit').data('rg');
                        var diabuscado = $('#mod-edit').data('diabuscado');
                        window.location.href = '/AjustePonto/Editar?contratadoId=' + contratadoId + '&nome=' + nome + '&rg=' + rg + '&ano=' + ano + '&mes=' + mes + '&dia=' + dia + "&diaBuscado=" + diabuscado;
                    }
                },
                error: function (response) {
                    alert('error ao excluir');
                }
            });
        }
    });

    // OnChange para Atualizar Hora (cores)
    $('#HoraRegistro').change(function () {
        var result = ValidaHora($('#HoraRegistro').val());

        if (result) {
            $('#HoraRegistro').css('border-color', '#bdc0c7');
            $('label[for="inputHora"]').css('color', '#666');
        }
        else {
            //  alert("Erro");
            $('#HoraRegistro').css('border-color', '#ea4335');
            $('label[for="inputHora"]').css('color', '#ea4335');
        }
    });

    $('#frmAjustePonto').submit(function () {
        var resultForm = false;
        var resultForm = ValidaHora($('#HoraRegistro').val());
        return resultForm;
    });

    $('#btnCancelarPonto').click(function () {
        LimpaCampos();
    });
});

function ValidaHora(hora) {
    if (hora.length == 8) {
        if (parseInt(hora.substring(0, 2)) >= 0 && parseInt(hora.substring(0, 2)) < 24) {
            if (parseInt(hora.substring(5, 3)) >= 0 && parseInt(hora.substring(5, 3)) < 60) {
                if (parseInt(hora.substring(8, 6)) >= 0 && parseInt(hora.substring(8, 6)) < 60) {
                    return true;
                }
            }
        }
    }

    return false;
}

function ValidaModal() {
    resultModal = false;
    var result = ValidaHora($('#inputHora').val());

    if (result && $('#selectRegistro').val() != '-- Selecione --') {
        resultModal = true;
    }
    return resultModal;
}

function msgAtencao() {
    $.gritter.add({ title: "Atenção", text: "Não existem contratados cadastrados para esta empresa.", class_name: "color warning" });
}

function HabilitarCampos() {
    $('#ContratadoId').prop("disabled", false);
    $('#Ano').prop("disabled", false);
    $("#Mes").prop("disabled", false);
    $("#Dia").prop("disabled", false);
    $("#btnBuscar").prop("disabled", false);
}

function DesabilitarCampos() {
    $('#ContratadoId').prop("disabled", true);
    $('#Ano').prop("disabled", true);
    $("#Mes").prop("disabled", true);
    $("#Dia").prop("disabled", true);
    $("#btnBuscar").prop("disabled", true);
}

function LimpaCampos() {

    $('#HoraRegistro').val('');
    $('#selectRegistro').val('');
}