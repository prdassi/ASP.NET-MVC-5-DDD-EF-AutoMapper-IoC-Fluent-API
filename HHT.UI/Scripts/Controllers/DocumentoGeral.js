$(document).ready(function () {
    //Ativar sub-mennu
    $('#admin').addClass('active open');
    $('#adminDocumentosGerais').addClass('active');
    if ($('#TipoDocumentoId').val() == 3 || $('#TipoDocumentoId').val() == 1) {
        $('#LocalId').val('0'); $
        $('#Local').removeAttr('required');
        $('#LocalId').removeAttr('required');
    }

    //Abrir modal para confirmar exclusão
    $('#datatable').on('click', '.btnExcluir', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        $('#mod-danger').data('id', id).modal('show');
    });

    $('#btnConfirmaExclusao').click(function () {
        var id = $('#mod-danger').data('id');

        $.ajax({
            url: '/DocumentoGeral/Excluir',
            data: { "documentoId": id },
            type: "POST",
            cache: false,
            success: function (response) {

                if (response.documentoVinculado) {
                    $("<h5><span id='msg-alerta' class='label label-danger text-center' style='font-size: 13px'>Não foi possível excluir este documento, existem Empresas/Contratados vinculados!</span></h5>").insertAfter($('#mod-danger').find('h4'));
                    $('#btnConfirmaExclusao').hide();
                    $('#mod-danger').modal({ "backdrop": "static" });
                }
                else {
                    //Remove a linha visual da lista de usuários
                    var table = $('#datatable').DataTable();
                    table.row($('#datatable').find('#row_' + id)).remove().draw();
                    $('#mod-danger').modal('hide');
                }

            },
            error: function (response) {
                alert('error');
            }
        });
    });

    $('#bntCancelar, #btnClose, .mdi.mdi-close').on('click', function () {
        $('#msg-alerta').remove();
        $('#btnConfirmaExclusao').show();
        $('#btnConfirmaExclusaoEmpresaDocumentos').remove();
    });

    $('select[name=TipoDocumentoId]').change(function () {
        var value = $(this).val();
        if (value == 2) //Integração
        {
            $('#Local').attr('required', 'required');
            $('#Nome').val('');
            $('#radioButtonOpcao').show();
        }
        else {
            $('input[name=RadioButton]').attr('checked', false);
            $('#radioButtonOpcao').hide();
            $('#Nome').val('');
            $('#Nome').attr('readonly', false);
            $('#selectLocal').hide();
            $('#Local').removeAttr('required');
            $('#LocalId').removeAttr('required');
            $('#LocalId').val('0');
        }
    })

    $('input:radio[name="RadioButton"]').change(function () {
        var text = $(this).attr('id');
        $('#Nome').val(text);
        $('#Nome').attr('readonly', true);

        if (text == 'Local') {
            $('#LocalId').val('');
            $('#LocalId').attr('required', 'required');
            $("#LocalId option[value='1']").remove();
            $('#selectLocal').show();
            $("#selectLocal option[value='0']").remove();

        }
        else {
            $("#LocalId").append(new Option("", "0"));
            $('#LocalId').removeAttr('required');
            $('#LocalId').val('0');//
            $('#selectLocal').hide();
        }
    })

    //Editar
    if ($('#TipoDocumentoId').val() == 2) {
        if ($('#Nome').val() == 'Completa') {
            $('input:radio[name=RadioButton][id=Completa]').attr('checked', true);
            $('#Local').removeAttr('required');
        }
        else {
            $('input:radio[name=RadioButton][id=Local]').attr('checked', true);
            $('#selectLocal').show();
        }

        $('#radioButtonOpcao').show();
        $('#Nome').attr('readonly', true);
    }

    $('#btnCancelarCadastrar').click(function () {
        // reset form 
        $('#frmrDocumentoGeral')[0].reset()
        $('#frmrDocumentoGeral').validator('destroy').validator()
        $('#radioButtonOpcao').hide();
        $('#Local').removeAttr('required');
        $('#LocalId').removeAttr('required');
        $('#selectLocal').hide();
    });

    $('#btnOk').click(function () {
        // reset form 
        $('#mod-primary').modal('hide');
    });
    
    $('#frmrDocumentoGeral').validator().on('submit', function (e) {
        if (e.isDefaultPrevented()) {

        }
        else {
            $('#AssociadoId').first().attr("disabled", false);
            $('#TipoDocumentoId').attr("disabled", false);
            $('#radioButtonOpcao').attr("disabled", false);
            $('#Local').attr("disabled", false);
            $('#selectLocal').attr("disabled", false);
            $('#LocalId').attr("disabled", false);
            $('#Completa').attr("disabled", false);
        }
    });

    $('#AssociadoId').change(function () {
        $("#TipoDocumentoId").html("");
        $.getJSON('/DocumentoGeral/ListaTipoDocumento', { associado: $("#AssociadoId option:selected").text() }, function (data) {
            $('#TipoDocumentoId').append('<option value="">-- Selecione --</option>');
            $.each(data, function (index, item) {
                $('#TipoDocumentoId').append('<option value=' +
                  item.TipoDocumentoId + '>' + item.Nome + '</option>');
            });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert('Error getting lista!');
        });
    });
});

function botaoNovoDocumento() {
    //Inserir botão dinâmico de cadastro
    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");
    $(".row.be-datatable-header").find(".col-sm-6:first").prepend("<div class='button-left'><a href='/DocumentoGeral/Cadastrar' class='btn btn-primary'><i class='icon color icon-left mdi mdi-plus-circle-o'></i> Novo Documento</a></div>");
}

function DocumentoVinculado() {
    var id = $('#hiddenDocumentoId').val();

    $.ajax({
        url: '/DocumentoGeral/DocumentoVinculado',
        data: { "documentoId": id },
        type: "Post",
        cache: false,
        success: function (response) {

            if (response.documentoVinculado) {
                //   $("<h5><span id='msg-alerta' class='label label-danger text-center' style='font-size: 13px'>Não será possível alterar alguns campos deste documento, existem Empresas/Contratados vinculados!</span></h5>").insertAfter($('#mod-danger').find('h4'));
                $('#mod-primary').modal('show');
                $('#AssociadoId').attr('readonly', true);
                $('#AssociadoId').first().attr("disabled", true);

                $('#TipoDocumentoId').attr('readonly', true);
                $('#TipoDocumentoId').attr("disabled", true);

                $('#radioButtonOpcao').attr('readonly', true);
                $('#radioButtonOpcao').attr("disabled", true);

                $('#Local').attr('readonly', true);
                $('#Local').attr("disabled", true);

                $('#selectLocal').attr('readonly', true);
                $('#selectLocal').attr("disabled", true);

                $('#LocalId').attr('readonly', true);
                $('#LocalId').attr("disabled", true);

                $('#Completa').attr('readonly', true);
                $('#Completa').attr("disabled", true);

                var tipo = $('#TipoDocumentoId').val();

                $('#Local').removeAttr('required');
                $('#LocalId').removeAttr('required');

            }
        },
        error: function (response) {

        }
    });
}