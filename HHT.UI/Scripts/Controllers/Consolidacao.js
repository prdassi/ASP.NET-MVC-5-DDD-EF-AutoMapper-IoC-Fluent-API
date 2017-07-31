$(document).ready(function () {

    //Ativar sub-mennu
    $('#relatorio').addClass('active open');
    $('#relatorioConsolidacao').addClass('active');

    $('#LocalId').val('');
    $('#Ano').val('');
    
    $('#datatable').dataTable({
        destroy: true,
        "searching": false,
        "scrollX": true,
        "autoWidth": false,
        dom: 'Bfrtip',
        buttons: ['excel', 'pdf']
    });
    
    //$('#datatable thead tr:eq(0) th:eq(2)').html("This is a really long column title!");
    //$('#datatable thead th[colspan]').wrapInner('<span/>').append('&nbsp;');

    $('.row.be-datatable-header').remove();
    $('.row.be-datatable-footer').remove();

    $(".buttons-excel, .buttons-pdf").css('margin-top', '10px');
    $(".buttons-pdf").css('margin-left', '10px');
    $(".dataTables_scrollHead").css('margin-top', '0px');

    $(".dataTables_info").hide();
    $(".dataTables_paginate.paging_simple_numbers").hide();

    $('#LocalId').change(function () {
        HabilitarCampos();
        $("#EmpresaId :gt(0)").remove();
        if ($('#LocalId').val() > 0 || $('#LocalId').val() != '') {
            $.getJSON('/InconsistenciaHorario/ListaEmpresas', { localId: $('#LocalId').val() }, function (data) {
                if (data.length > 0) {
                    $('#EmpresaId').append('<option value="0">Todas</option>');
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
        $.getJSON('/Consolidacao/ListaMeses', { ano: $('#Ano').val() }, function (data) {
            $('#Mes').append('<option value="0">Todos</option>');
            $.each(data, function (index, item) {
                $('#Mes').append('<option value=' +
                  item.mesId + '>' + item.nome + '</option>');
            });
        }).fail(function (jqXHR, textStatus, errorThrown) {
            alert('Error getting mêses!');
        });
    });
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