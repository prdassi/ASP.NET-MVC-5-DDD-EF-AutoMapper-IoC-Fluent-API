$.fn.dataTable.Buttons.swfPath = '//cdn.datatables.net/buttons/1.0.0/swf/flashExport.swf';

$(document).ready(function () {
    $('#LocalId').val("");
    $('#Ano').val("");

    //Ativar sub-mennu
    $('#relatorio').addClass('active open');
    $('#relatorioInconsistencia').addClass('active');

    //Remover o item vazio (importante no cadastro de documentos gerais)
    $("#LocalId option[value='1']").remove();

    $('#LocalId').change(function () {
        HabilitarCampos();
        $("#EmpresaId :gt(0)").remove();
        if ($('#LocalId').val() > 0 || $('#LocalId').val() != '') {
            $.getJSON('/InconsistenciaHorario/ListaEmpresas', { localId: $('#LocalId').val() }, function (data) {
                if (data.length > 0) {
                    $.each(data, function (index, item) {
                        $('#EmpresaId').append('<option value=' +
                          item.empresaId + '>' + item.nome + '</option>');
                    });
                }
                else { DesabilitarCampos(); }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert('Error getting empresas!');
            });
        }
    });

    $('#Ano').change(function () {
        $("#Mes :gt(0)").remove();
        $.getJSON('/InconsistenciaHorario/ListaMeses', { ano: $('#Ano').val() }, function (data) {
            $.each(data, function (index, item) {
                $('#Mes').append('<option value=' +
                  item.mesId + '>' + item.nome + '</option>');
            });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert('Error getting mêses!');
        });
    });

    $('#tableExport').DataTable({
        dom: 'Bfrtip',
        buttons: [
             'excel', 'pdf'
        ],
        "bStateSave": true,
        "bLengthChange": false,
        "oLanguage": {
            "sInfoFiltered": "",
            "sSearch": "Buscar:",
            "sInfo": "Exibindo _TOTAL_ registro(s)",
            "sInfoEmpty": "Nenhum registro encontrado",
            "sProcessing": "Buscando...",
            "sLoadingRecords": "Por favor aguarde...",
            "oPaginate": {
                "sFirst": "Primeiro",
                "sPrevious": "Anterior",
                "sNext": "Pr&oacute;ximo",
                "sLast": "&Uacute;ltimo"
            },
        }
    });

    $('#tableExport_wrapper').find('.dt-buttons.btn-group').css('float', 'left');
    $('#tableExport_wrapper').find('.dt-buttons.btn-group').css('padding', '15px 0px 24px 25px');

    $('#tableExport_filter').css('float', 'right');
    $('#tableExport_filter').css('padding-top', '17px');
    $('#tableExport_filter').css('padding-bottom', '24px');
    $('#tableExport_filter').css('padding-right', '25px');

    $('#tableExport').css('border-top', '1px solid #d5d8de');
    $('#tableExport').css('padding', '8px 0px 0px');

    $("#tableExport_filter").find('.form-control').removeClass("input-sm").addClass("input-xs");

    $(".buttons-pdf").css('margin-left', '15px');

    $(".dataTables_info").css('float', 'left');

    $("#tableExport_paginate").css('margin', '0px');
    $("#tableExport_paginate").css('border-top', '1px solid #d5d8de');
    $("#tableExport_paginate").css('background-color', '#f7f7f7');
    $("#tableExport_paginate").css('padding', '15px 3px 13px');

    $("#tableExport_info").css('padding', '25px 0px 13px 18px');

    $("#tableExport_paginate").css('padding', '15px 18px 13px');
});



function HabilitarCampos() {
    $('#EmpresaId').prop("disabled", false);
    $('#Ano').prop("disabled", false);
    $("#Mes").prop("disabled", false);
    $("#btnBuscar").prop("disabled", false);
}

function DesabilitarCampos() {
    $('#EmpresaId').prop("disabled", true);
    $('#Ano').prop("disabled", true);
    $("#Mes").prop("disabled", true);
    $("#btnBuscar").prop("disabled", true);
    msgAtencao();
}

function msgAtencao() {
    $.gritter.add({ title: "Atenção", text: "Não existem Empresas cadastradas para este Local.", class_name: "color warning" });
}