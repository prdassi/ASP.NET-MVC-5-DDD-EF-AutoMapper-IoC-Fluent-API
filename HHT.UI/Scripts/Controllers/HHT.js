$(document).ready(function () {

    //Ativar mennu
    $('#hht').addClass('active');
    $(".row.be-datatable-header").find('.form-control').removeClass("input-sm").addClass("input-xs");

    $('#LocalId').change(function () {
        var localId = $('#LocalId').val();
        if (localId != "")
            $("#RegistroPonto").removeClass("has-error has-danger");
        else
            $("#RegistroPonto").addClass("has-error has-danger");
    });
   
    if ($('#RG').val() == "" && $('#Nome').val() == "") {
        $('#RG').focus();
    }
});

$('#RG').on('focus', function () {
    $('#Nome').val('');
});

$('#Nome').on('focus', function () {
    $('#RG').val('');
});

function RegistrarPonto(registrarPonto) {
    var localId = $('#LocalId').val();
    if (localId != "") {
        var localNome = $('#LocalId option:selected').text()
        window.location.href = '/HHT/RegistrarPonto?registrarPonto=' + registrarPonto + '&localId=' + localId + '&localNome=' + localNome;
    }
    else {
        $("#RegistroPonto").addClass("has-error has-danger");
    }
}