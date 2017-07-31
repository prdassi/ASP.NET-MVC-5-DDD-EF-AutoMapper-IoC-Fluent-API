$(document).ready(function () {

    //Ativar sub-menu
    $('#portal').addClass('active open');
    $('#portalEmpresa').addClass('active');

    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");

    //Abrir modal para confirmar exclusão

    $('#datatable').on('click', '.btnExcluir', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        $('#mod-danger').data('id', id).modal('show');

        var empresaId = $(this).data('empresaid');
        $('#mod-danger').data('empresaid', empresaId).modal('show');
    });


    $('#btnConfirmaExclusao').click(function () {
        var id = $('#mod-danger').data('id');

        $.ajax({
            url: '/Empresa/Excluir',
            data: { empresaId: id },
            type: "POST",
            cache: false,
            success: function (response) {
                if (response.usuarioVinculado) {
                    $("<h5><span id='msg-alerta' class='label label-danger text-center' style='font-size: 13px'>Não foi possível excluir esta empresa, existem Usuários vinculados!</span></h5>").insertAfter($('#mod-danger').find('h4'));
                    $('#btnConfirmaExclusao').hide();
                    $('#mod-danger').modal({ "backdrop": "static" });
                }
                else if (response.contratadoVinculado) {
                    $("<h5><span id='msg-alerta' class='label label-danger text-center' style='font-size: 13px'>Não foi possível excluir esta empresa, existem Contratados vinculados!</span></h5>").insertAfter($('#mod-danger').find('h4'));
                    $('#btnConfirmaExclusao').hide();
                    $('#mod-danger').modal({ "backdrop": "static" });
                }
                else if (response.usuarioEContratadoVinculado) {
                    $("<h5><span id='msg-alerta' class='label label-danger text-center' style='font-size: 13px'>Não foi possível excluir esta empresa, existem Usuários e Contratados vinculados!</span></h5>").insertAfter($('#mod-danger').find('h4'));
                    $('#btnConfirmaExclusao').hide();
                    $('#mod-danger').modal({ "backdrop": "static" });
                }
                else if (response.documentoVinculado) {
                    $("<div id='msg-aviso' class='label-danger'><span id='msg-alerta' class='label text-center' style='font-size: 13px'>Existem documentos vinculados a esta Empresa, ao confirmar,<br>será excluída a empresa e todos os seus respectivos documentos!</span></div>").insertAfter($('#mod-danger').find('h4'));
                    $('#btnConfirmaExclusao').hide();
                    $('#btnConfirmaExclusaoEmpresaCompleta').show();
                    $('#mod-danger').modal({ "backdrop": "static" });
                }
                else if (response.empresaExcluida) {
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

    $('#btnConfirmaExclusaoEmpresaCompleta').click(function () {
        var id = $('#mod-danger').data('id');

        $.ajax({
            url: '/Empresa/ExcluirEmpresaCompleta',
            data: { empresaId: id },
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
            },
            after: function (responde) {                
                $('#msg-aviso').remove();
                $('#btnConfirmaExclusaoEmpresaCompleta').remove();
            }   
        });

        $('#btnConfirmaExclusao').show();
        $('#msg-alerta').remove();
        $('#btnConfirmaExclusaoEmpresaCompleta').hide();
    });

    $("#close-sucess").on('click', function () {
        var empresaId = $('#EmpresaId').val();
        window.location.href = '/Empresa/Documentos?empresaId=' + empresaId;
    });

    $('#bntCancelar, #btnClose').on('click', function () {
        $('#btnConfirmaExclusao').show();
        $('#msg-alerta').remove();
        $('#btnConfirmaExclusaoEmpresaCompleta').hide();
    });

    $('#btnConfirmaExclusaoDocumento').click(function () {
        var id = $('#mod-danger').data('id');
        var empresaId = $('#mod-danger').data('empresaid');

        $.ajax({
            url: '/Empresa/ExcluirDocumento',
            data: { "arquivoEmpresaId": id, "empresaId": empresaId },
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

    $('#btnSim').click(function () {
        $('#DocumentoGeralId').val('')
        $('#DataDocumento').val('');
    });

    //Remover o item vazio (importante no cadastro de documentos gerais)
    $("#LocaisSelecionados option[value='1']").remove();

    //Cadastro
    $('input:radio[name="Contrato"]').change(function () {
        var text = $(this).attr('id');
       
        if (text == 'ContratoSim') {
            $("#aes").show();

            // adiciona validação 
            $("#NomeGestorAES").attr("required", "true");
            $("#EmailGestorAES").attr("required", "true");
            $("#NomeMonitorAES").attr("required", "true");
            $("#EmailMonitorAES").attr("required", "true");
        }
        else {
            $("#aes").hide();
            $("#NomeGestorAES, #EmailGestorAES, #NomeMonitorAES, #EmailMonitorAES").val("");


            // retira validação 
            $('#NomeGestorAES').removeAttr('required');
            $('#EmailGestorAES').removeAttr('required');
            $('#NomeMonitorAES').removeAttr('required');
            $('#EmailMonitorAES').removeAttr('required');
        }
    })

    // Cancelar Cadastrar (Inclusão: Limpar Campos)
    $('#btnCancelarCadastrar').click(function () {
        // reset Formulário 
        $('#frmCadastrar')[0].reset();
        $('#frmCadastrar').validator('destroy').validator();

        // reset MultiselectList
        $('#ServicosSelecionados').multiselect('deselectAll', false);
        $('#ServicosSelecionados').multiselect('updateButtonText');
        $('#MultiSelectServicosSelecionados').find('button').first().css('border-color', '#bdc0c7');
        $('#labelTipoServico').css('color', '#666');


        $('#LocaisSelecionados').multiselect('deselectAll', false);
        $('#LocaisSelecionados').multiselect('updateButtonText');
        $('#MultiSelectLocaisSelecionados').find('button').first().css('border-color', '#bdc0c7');
        $('#labelTipoLocal').css('color', '#666');
        $('#lbllocal').css('color', '#666');
   

        // reset AES
        $("#aes").hide();
    });

    // OnChange para Atualizar Validação (cores)
    $('#ServicosSelecionados').on('change', function () {
        if ($('#ServicosSelecionados').val() == null) {
            $('#MultiSelectServicosSelecionados').find('button').first().css('border-color', 'red');
            $('#labelTipoServico').css('color', '#ea4335');
        }
        else {
            $('#MultiSelectServicosSelecionados').find('button').first().css('border-color', '#bdc0c7');
            $('#labelTipoServico').css('color', '#666');
        }
    });
    $('#LocaisSelecionados').on('change', function () {
        if ($('#LocaisSelecionados').val() == null) {
            $('#MultiSelectLocaisSelecionados').find('button').first().css('border-color', 'red');
            $('#labelTipoLocal').css('color', '#ea4335');
        }
        else {
            $('#MultiSelectLocalsSelecionados').find('button').first().css('border-color', '#bdc0c7');
            $('#labelTipoLocal').css('color', '#666');
        }
    });
    // Validação para MultiSelectList
    $('#frmCadastrar').validator().on('submit', function (e) {
        var result = true;

        if ($('#ServicosSelecionados').val() == null) {
            $('#MultiSelectServicosSelecionados').find('button').first().css('border-color', 'red');
            $('#labelTipoServico').css('color', '#ea4335');
     
            result = false;
        }


        if ($('#LocaisSelecionados').val() == null) {
            $('#MultiSelectLocaisSelecionados').find('button').first().css('border-color', 'red');
            $('#labelTipoLocal').css('color', '#ea4335');
            $('#lbllocal').css('color', '#ea4335');
            result = false;
        }

        if (!$("#frmCadastrar").data("bs.validator").validate().hasErrors() && result) {
            $("frmCadastrar").submit();
        }
        else {
            return false;
        }
    })

    // OnChange para Atualizar Validação (cores)
    $('#upload').on('change', function () {
        if ($('#upload').val() == "") {
            $('#incluirDocumento').css('border-color', 'red');            
        }
        else {
            $('#incluirDocumento').css('border-color', '#bdc0c7');
        }
    });

    // OnChange para Atualizar Validação (cores)
    $('#DataDocumento').on('change', function () {
        var result = validaData($('#DataDocumento'));

        if (result) {
            $('#DataDocumento').css('border-color', '#bdc0c7');
            $('label[for="inputNomeResponsavel"]').css('color', '#666');
        }
        else {
            $('#DataDocumento').css('border-color', 'red');
            $('label[for="inputNomeResponsavel"]').css('color', '#ea4335');
        }
    });

    // Validação para Documento
    $('#frmDocumentos').validator().on('submit', function (e) {
        var result = validaData($('#DataDocumento'));

        if (result) {
            $('#DataDocumento').css('border-color', '#bdc0c7');
            $('label[for="inputNomeResponsavel"]').css('color', '#666');

        }
        else {
            $('#DataDocumento').css('border-color', 'red');
            $('label[for="inputNomeResponsavel"]').css('color', '#ea4335');
        }

        if ($('#upload').val() == "") {
            $('#incluirDocumento').css('border-color', 'red');

            result = false;
        }

        if (!$("#frmDocumentos").data("bs.validator").validate().hasErrors() && result) {
            $("frmDocumentos").submit();
        }
        else {
            return false;
        }
    });


    // Validação para Documento
    $('#frmEditarDocumentos').validator().on('submit', function (e) {
        var result = validaData($('#DataDocumento'));

        if (result) {
            $('#DataDocumento').css('border-color', '#bdc0c7');
            $('label[for="Data_Admiss_o"]').css('color', '#666');
        }
        else {
            $('#DataDocumento').css('border-color', 'red');
            $('label[for="Data_Admiss_o"]').css('color', '#ea4335');
        }

        if (!$("#frmEditarDocumentos").data("bs.validator").validate().hasErrors() && result) {
            $("frmEditarDocumentos").submit();
        }
        else {
            return false;
        }
    });

    // Cancelar Cadastrar (Inclusão: Limpar Campos)
    $('#btnCancelarIncluirDocumento').click(function () {
        // reset Formulário 
        $('#frmDocumentos')[0].reset();
        $('#frmDocumentos').validator('destroy').validator();
        $('#DataDocumento').css('border-color', '#bdc0c7');
        $('#DataDocumento').val('');
        $('label[for="inputDocumento"]').css('color', '#666');
        $('label[for="inputNomeResponsavel"]').css('color', '#666');
        $('#incluirDocumento').css('border-color', '#bdc0c7');
        $('#incluirDocumento').find("span").first().text("Selecione o arquivo...");
    });
});

function FecharIncluirDocumento(empresaId) {
    window.location.href = '/Empresa/Documentos?empresaId=' + empresaId;
}

function botaoNovoDocumento(id) {
    //Inserir botão dinâmico de cadastro
    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");
    $(".row.be-datatable-header").find(".col-sm-6:first").prepend("<div class='button-left'><a href='/Empresa/ArquivoEmpresa?empresaId="+id+"' class='btn btn-primary'><i class='icon color icon-left mdi mdi-plus-circle-o'></i> Novo Documento</a></div>");
}

function botaoNovaEmpresa() {    
    //Inserir botão dinâmico de cadastro
    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");
    $(".row.be-datatable-header").find(".col-sm-6:first").prepend("<div class='button-left'><a href='/Empresa/Cadastrar' class='btn btn-primary'><i class='icon color icon-left mdi mdi-plus-circle-o'></i> Nova Empresa</a></div>");
}
