$(document).ready(function () {

    //Ativar sub-mennu
    $('#portal').addClass('active open');
    $('#portalContratado').addClass('active');

    $('#Local').removeAttr('required');
    $('#LocalId').removeAttr('required');

    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");

    //Abrir modal para confirmar exclusão

    $('#datatable').on('click', '.btnExcluir', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        $('#mod-danger').data('id', id).modal('show');

        var contratadoId = $(this).data('contratadoid');
        $('#mod-danger').data('contratadoid', contratadoId).modal('show');
    });

    $('select[name=TipoDocumentoId]').change(function () {
        var value = $(this).val();
        if (value == 2) //Integração
        {
            $('#LocalId').attr('required', 'required');
            $('#Local').attr('required', 'required');
            $('#LocalId').val('');

        }
        else {
            $('#divLocal').hide();
            $('#LocalId').hide();
            $('#Local').removeAttr('required');
            $('#LocalId').removeAttr('required');
            $('#LocalId').val('0');
        }
    })


    $('#TipoDocumentoId').change(function () {
        $("#DocumentoGeralId :gt(0)").remove();
        $.getJSON('/Contratado/ListaDocumentos', { TipoDocumentoId: $('#TipoDocumentoId').val() }, function (data) {
            $.each(data, function (index, item) {
                $('#DocumentoGeralId').append('<option value=' +
                  item.DocumentoGeralId + '>' + item.Nome + '</option>');
            });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert('Error getting documentos!');
        });
    });

    // Codigos do Cadastro/Editar
    if ($('#DocumentoGeralId').val() == 2)
    {
        $('#divLocal').show();
        $('#LocalId').show();
        $('#LocalId').attr('required', 'required');
        $('#Local').attr('required', 'required');
        $('#LocalId').val('');
        $('#inputLocal').show();
    }
    else {
        $('#divLocal').hide();
        $('#LocalId').hide();
        $('#Local').removeAttr('required');
        $('#LocalId').removeAttr('required');

    }

    $('select[name=DocumentoGeralId]').change(function () {

        var value = $('#TipoDocumentoId').val();
        var valueText = $("#DocumentoGeralId option:selected").text();
        var valueSearch = valueText.indexOf("Local");
        if (value == 2 && valueSearch != -1) {
            $('#LocalId').empty();
            $("#LocalId :gt(0)").remove();
            $.getJSON('/Contratado/LocalDocumentos', { DocumentoGeralId: $('#DocumentoGeralId').val() }, function (data) {
                $.each(data, function (index, item) {
                    $('#LocalId').append('<option value=' +
                      item.LocalId + '>' + item.Nome + '</option>');
                });
                $("#LocalId option[value='']").remove();
                //    $('#LocalId').first().attr("disabled", true);
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert('Error getting documentos!');
            });


            $('#divLocal').show();
            $('#LocalId').show();
            $('#LocalId').attr('required', 'required');
            $('#Local').attr('required', 'required');
            $('#LocalId').val('');
            $('#inputLocal').show();
        }
        else {
            $('#divLocal').hide();
            $('#LocalId').hide();
            $('#Local').removeAttr('required');
            $('#LocalId').removeAttr('required');
            $('#LocalId').val('1');
        }
    })

    
    // Fim Cadastro/Editar

    $('#btnConfirmaExclusao').click(function () {
        var id = $('#mod-danger').data('id');

        $.ajax({
            url: '/Contratado/Excluir',
            data: { contratadoId: id },
            type: "POST",
            cache: false,
            success: function (response) {
                if (response.documentoVinculado) {
                    $("<div id='msg-aviso' class='label-danger'><span id='msg-alerta' class='label text-center' style='font-size: 13px'>Existem documentos vinculados a este Contratado, ao confirmar,<br>será excluído o contratado e todos os seus respectivos documentos!</span></div>").insertAfter($('#mod-danger').find('h4'));
                    $('#btnConfirmaExclusao').hide();

                    $('#btnConfirmaExclusaoContratadoCompleto').show();

                    $('#mod-danger').modal({ "backdrop": "static" });
                }
                else if (response.contratadoExcluido) {
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

    $('#btnConfirmaExclusaoContratadoCompleto').click(function () {
        var id = $('#mod-danger').data('id');

        $.ajax({
            url: '/Contratado/ExcluirContratadoCompleto',
            data: { contratadoId: id },
            type: "POST",
            cache: false,
            success: function (response) {

                //Remove a linha visual da lista de usuários
                var table = $('#datatable').DataTable();
                table.row($('#datatable').find('#row_' + id)).remove().draw();

                $('#mod-danger').modal('hide');

                $('#msg-aviso').remove();
                $('#btnConfirmaExclusaoContratadoCompleto').remove();
                $('#btnConfirmaExclusao').show();
            },
            error: function (response) {
                alert('error ao excluir');
            }
        });
    });

    $('input:radio[name="Restricao"]').change(
     function () {
         if ($(this).is(':checked')) {
             var fg = $(this).val();
             var radio = $(this).attr('id');

             if (radio == 'restricaoNao') {
                 $('#divInativoJustificativa').hide();
                 $('#Justificativa').val("");
                 $('#Justificativa').removeAttr('required');
             }
             else {
                 $('#divInativoJustificativa').show();
                 $("#Justificativa").attr("required", "true");
             }
         }
     });

    $('#btnConfirmaExclusaoDocumento').click(function () {
        var id = $('#mod-danger').data('id');
        var contratadoId = $('#mod-danger').data('contratadoid');

        $.ajax({
            url: '/Contratado/ExcluirDocumento',
            data: { "arquivoContratadoId": id, "contratadoId": contratadoId },
            type: "POST",
            cache: false,
            success: function (response) {
                //Remove a linha visual da lista de usuários
                var table = $('#datatable').DataTable();
                table.row($('#datatable').find('#row_' + id)).remove().draw();

                $('#mod-danger').modal('hide');
            },
            error: function (response) {
                alert('error ao excluir');
            }
        });
    });

    //Tratamento do campo Restrição
    var radioId = $('input:radio[name="Restricao"]:checked').attr('id');
    if (radioId == 'restricaoNao') {
        $('#divInativoJustificativa').hide();
        $('#Justificativa').val("");
        $('#Justificativa').removeAttr('required');
    }
    else {
        $('#divInativoJustificativa').show();
        $("#Justificativa").attr("required", "true");
    }

    //Limpando os campos
    $('#btnSim').click(function () {
        $('#DocumentoGeralId').val('')
        $('#DataDocumento').val('');
        $('#Comentario').val('');
        $('#LocalId').val('');
        $('#TipoDocumentoId').val('');
        $('#upload').val('');
    });

    //Remover o item vazio (importante no cadastro de documentos gerais)
    $("#LocalId option[value='1']").remove();

    $("#btnGerarQRCode").click(function () {
        var nomeqrcode = $(this).data('nomeqrcode');

        $('#mod-primary').modal('hide');
        window.print();

        window.location.href = '/Contratado/';
    });

    // OnChange Cadastrar para Atualizar Validação (cores)
    $('#DataAdmissao').on('change', function () {
        var result = validaData($('#DataAdmissao'));

        if (result) {
            $('#DataAdmissao').css('border-color', '#bdc0c7');
            $('label[for="Data_Admiss_o"]').css('color', '#666');
        }
        else {
            $('#DataAdmissao').css('border-color', 'red');
            $('label[for="Data_Admiss_o"]').css('color', '#ea4335');
        }
    });

    // OnChange Cadastrar Documento para Atualizar Validação (cores)
    $('#DataDocumento').on('change', function () {
        var result = validaData($('#DataDocumento'));

        if (result) {
            $('#DataDocumento').css('border-color', '#bdc0c7');
            $('label[for="inputRealizado"]').css('color', '#666');
        }
        else {
            $('#DataDocumento').css('border-color', 'red');
            $('label[for="inputRealizado"]').css('color', '#ea4335');
        }
    });

    // OnChange Cadastrar Documento para Atualizar Validação (cores)
    $('#upload').on('change', function () {
        if ($('#upload').val() == "") {
            $('#incluirDocumento').css('border-color', 'red');
            $('label[for="inputDocumento"]').css('color', '#ea4335');
        }
        else {
            $('label[for="inputDocumento"]').css('color', '#666');
            $('#incluirDocumento').css('border-color', '#bdc0c7');
        }
    });

    // Cancelar Cadastrar (Inclusão: Limpar Campos)
    $('#btnCancelarCadastrar').click(function () {
        // reset Formulário 
        $('#frmCadastrar')[0].reset();
        $('#frmCadastrar').validator('destroy').validator();
        $('#DataAdmissao').css('border-color', '#bdc0c7');
        $('label[for="Data_Admiss_o"]').css('color', '#666');
    });

    // Cancelar Cadastrar Documento (Inclusão: Limpar Campos)
    $('#btnCancelarCadastrarDocumento').click(function () {
        // reset Formulário 
        $('#frmCadastrarDocumento')[0].reset();
        $('#frmCadastrarDocumento').validator('destroy').validator();
        $('#DataDocumento').css('border-color', '#bdc0c7');
        $('label[for="inputRealizado"]').css('color', '#666');
        $('label[for="inputDocumento"]').css('color', '#666');
        $('#incluirDocumento').css('border-color', '#bdc0c7');
        $('#incluirDocumento').find("span").first().text("Selecione o arquivo...");
    });

    // Validar Cadastrar
    $('#frmCadastrar').validator().on('submit', function (e) {
        var result = validaData($('#DataAdmissao'));

        if (result) {
            $('#DataAdmissao').css('border-color', '#bdc0c7');
            $('label[for="Data_Admiss_o"]').css('color', '#666');
        }
        else {
            $('#DataAdmissao').css('border-color', 'red');
            $('label[for="Data_Admiss_o"]').css('color', '#ea4335');
        }

        if (!$("#frmCadastrar").data("bs.validator").validate().hasErrors() && result) {
            $("frmCadastrar").submit();
        }
        else {
            return false;
        }
    })

    // Validar Cadastrar Documento
    $('#frmCadastrarDocumento').validator().on('submit', function (e) {
        var result = validaData($('#DataDocumento'));

        if (result) {
            $('#DataDocumento').css('border-color', '#bdc0c7');
            $('label[for="inputRealizado"]').css('color', '#666');

        }
        else {
            $('#DataDocumento').css('border-color', 'red');
            $('label[for="inputRealizado"]').css('color', '#ea4335');
        }

        if ($('#upload').val() == "" && $('#Arquivo').val() == undefined) {
            $('#incluirDocumento').css('border-color', 'red');
            $('label[for="inputDocumento"]').css('color', '#ea4335');

            result = false;
        }
        else {
            $('label[for="inputDocumento"]').css('color', '#666');
            $('#incluirDocumento').css('border-color', '#bdc0c7');
        }

        if (!$("#frmCadastrarDocumento").data("bs.validator").validate().hasErrors() && result) {
            $("frmCadastrarDocumento").submit();
        }
        else {
            return false;
        }
    })

    $("#close-sucess").on('click', function () {
        var contratadoId = $('#ContratadoId').val();
        window.location.href = '/Contratado/Documentos?contratadoId=' + contratadoId;
    });
});

function FecharQRCode() {
    window.location.href = '/Contratado/';
}

function FecharIncluirDocumento(contratadoId) {
    window.location.href = '/Contratado/Documentos?contratadoId=' + contratadoId;
}

function botaoNovoDocumento(id) {
    //Inserir botão dinâmico de cadastro
    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");
    $(".row.be-datatable-header").find(".col-sm-6:first").prepend("<div class='button-left'><a href='/Contratado/ArquivoContratado?contratadoId=" + id + "' class='btn btn-primary'><i class='icon color icon-left mdi mdi-plus-circle-o'></i> Novo Documento</a></div>");
}

function botaoNovoContratado() {
    //Inserir botão dinâmico de cadastro
    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");
    $(".row.be-datatable-header").find(".col-sm-6:first").prepend("<div class='button-left'><a href='/Contratado/Cadastrar' class='btn btn-primary'><i class='icon color icon-left mdi mdi-plus-circle-o'></i> Novo Contratado</a></div>");
}