$(document).ready(function () {

    $('#usuario').addClass('active');

    //Abrir modal para confirmar exclusão

    $('#datatable').on('click', '.btnExcluir', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        $('#mod-danger').data('id', id).modal('show');
    });


    $('#datatable').on('click', '.btnAcesso', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var perfil = $(this).data('perfil');
        var email = $(this).data('email');

        $('#mod-acesso').data('id', id);
        $('#mod-acesso').data('perfil', perfil);
        $('#mod-acesso').data('email', email);

        $('#mod-acesso').data('id', id).modal('show');

        if (perfil != "") {
           // $("#PerfilId option:selected").text = perfil;
            var el = document.getElementById("PerfilId");
            for (var i = 0; i < el.options.length; i++) {
                if (el.options[i].text == perfil) {
                    el.selectedIndex = i;
                }
            }
        }
        else {
            $("#PerfilId").val('');
        }
    });

    $('#btnConfirmaExclusao').click(function () {
        var id = $('#mod-danger').data('id');

        $.ajax({
            url: '/Usuario/Excluir',
            data: { usuarioId: id },
            type: "POST",
            cache: false,
            success: function (response) {
                if (response.contratadoVinculado) {
                    $("<h5><span id='msg-alerta' class='label label-danger text-center' style='font-size: 13px'>Não foi possível excluir este usuário, existem Contratados vinculados!</span></h5>").insertAfter($('#mod-danger').find('h4'));
                    $('#btnConfirmaExclusao').hide();
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

    $('#btnCancelar, #btnClose, .mdi.mdi-close').on('click', function () {
        $('#btnConfirmaExclusao').show();
        $('#msg-alerta').remove();
        $('#msg-acesso').remove();
        $('#mod-danger').data('id', id).modal('show');
        $("#PerfilId").val("");
    });


    $('#btnCancelarModal, #btnCloseModal, .mdi.mdi-close').on('click', function () {
        $('#btnConfirmaExclusao').show();
        $('#msg-alerta').remove();
        $('#msg-acesso').remove();
        $('#mod-danger').data('id', id).modal('show');
        $("#PerfilId").val("");
    });

    
    //Selecionar vários tipos de locais
    $('#LocaisSelecionados').multiselect({
        enableClickableOptGroups: true
    });

    $('input:radio[name="Porteiro"]').change(function () {
        var text = $(this).attr('id');
        if (text == 'Sim') {
            $("#inputUsuario").show();
            $("#LocaisSelecionados").show();
            $("#Locais").show();
            $("#LocaisSelecionados option:selected").removeAttr("selected");
            $("#LocaisSelecionados").multiselect('refresh');
        }
        else {
            $("#inputUsuario").hide();
            $("#LocaisSelecionados").hide();
            $("#Locais").hide();
        }
    })

    var porteiroId = $('input:radio[name="Porteiro"]:checked').attr('id');
    if (porteiroId == 'Sim') {
        $("#inputUsuario").show();
        $("#LocaisSelecionados").show();
        $("#Locais").show();
        $('#LocaisSelecionados').multiselect('refresh');
        $(".multiselect").attr('required', 'required');

    }
    else {
        if (porteiroId == "") {
            $('#divPorteiro').addClass('has-error has-danger');
        }
        $("#inputUsuario").hide();
        $("#LocaisSelecionados").hide();
        $("#Locais").hide();
        $('#LocaisSelecionados').removeAttr('required');
        $(".multiselect").removeAttr('required');
    }

    $('#LocaisSelecionados').on('change', function () {

        if ($("#LocaisSelecionados").multiselect().val() == null) {
            $('#divMultiSelect').find('button').first().css('border-color', 'red');
            $('#inputUsuario').css('color', '#ea4335');

        }
        else {
            $('#divMultiSelect').find('button').first().css('border-color', '#bdc0c7');
            $('#inputUsuario').css('color', '#666');
        }
    });

    $('#frmCadastrar').validator().on('submit', function (e) {
        var value = $('input:radio[name="Porteiro"]:checked').val();
        var result = true;

        if ($("#LocaisSelecionados").multiselect().val() == null && "True" == value) {
            $('#divMultiSelect').find('button').first().css('border-color', 'red');
            $('#inputUsuario').css('color', '#ea4335');
            result = false;
        }
        else {
            $('#divMultiSelect').find('button').first().css('border-color', '#bdc0c7');
            $('#inputUsuario').css('color', '#666');
            resultt = true;
        }

        if ($('#inputPassword').val().length >= 6) {
            $('#divSenha').removeClass('has-error has-danger');
            result = true;
        }
        else {
            if ($('#inputPassword').val().length >= 1) {
                $('#divSenha').addClass('has-error has-danger');
                $('#divSenha').first().css('border-color', 'red');
                msgAtencao();
                result = false;
            }
        }

        return result;
    });

    $('#btnCancelarCadastrar').click(function () {
        // reset form 
        $('#frmCadastrar')[0].reset()
        $('#frmCadastrar').validator('destroy').validator()
        $("#inputUsuario").hide();
        $("#LocaisSelecionados").hide();
        $("#Locais").hide();
    });


    $("#btnConfirmaAcesso").click(function (e) {
        if ($('#PerfilId').val() == "") {
            $('#PerfilId').css('border-color', '#bdc0c7');
        }
        else {
            var id = $('#mod-acesso').data('id');
            var nome = $("#PerfilId option:selected").text();
            var idPerfil = $("#PerfilId").val();
            var email = $('#mod-acesso').data('email');

            $.ajax({
                url: '/Usuario/Acesso',
                data: { usuarioid: id, perfil: nome, idPerfil: idPerfil, email: email },
                type: "POST",
                cache: false,
                async: true,
                success: function (response) {
                    if (response.usuario == "Próprio") {
                        $("<div class='modal-body'><div class='form-group col-md-12 xs-mt-10'><span class='text-center' style='font-size: 14px;'>Seu perfil foi alterado!<br />Você será redirecionado para fazer o login novamente em alguns segundos...</span></div></div><div class='modal-footer xs-mr-15'></div>").insertAfter($('#perfilAcesso'));
                        $('#perfilAcesso').hide();
                        $('#btnClose').hide();
                        $('#mod-acesso').modal({ "backdrop": "static" });
                        setTimeout(
                              function () {
                                  window.location.href = '/Login/Sair';
                              }, 5000);
                    }
                    else {
                        $('#mod-acesso').modal('hide');
                    }
                },
                error: function (response) {
                    alert('error');
                }
            });
        }
    });

});

function botaoNovoUsuario() {
    //Inserir botão dinâmico de cadastro
    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");
    $(".row.be-datatable-header").find(".col-sm-6:first").prepend("<div class='button-left'><a href='/Usuario/Cadastrar' class='btn btn-primary'><i class='icon color icon-left mdi mdi-plus-circle-o'></i> Novo Usuário</a></div>");
}


function msgAtencao() {
    $.gritter.add({ title: "Atenção", text: "O campo senha precisa ter no mínimo 6 caracteres.", class_name: "color danger" });
}


