$(function () {
    $('#datat').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                className: "btn btn-large btn-success"
            },
            {
                extend: 'excelHtml5',
                className: "btn btn-large btn-warning"
            }
        ]
    });
    $('.select2').select2({
        liveSearch: true,
        width: 'resolve'
    });
    $('.date-picker').datepicker({
        beforeShow: function (input, inst) {
            setTimeout(function () {
                inst.dpDiv.css({
                    top: $(".datepicker").offset().top + 35,
                    left: $(".datepicker").offset().left
                });
            }, 0);
        },
        format: "mm-yyyy",
        startView: "months",
        minViewMode: "months"
    });
    
    $('[data-toggle=confirmation]').confirmation({
        rootSelector: '[data-toggle=confirmation]'        
    });
    $("#tipo_art").on("change", function () {
        var value = $("option:selected", this).text();
        if (value == "Indizado") {
            $("#ind").removeAttr('disabled');
        }
        else {
            $("#ind").attr('disabled', 'disabled');

        }
    })
})



function validateUser() {
    var nombre = document.getElementById('usrnm');
    if (document.getElementById('usrnm').value.length < 5) {
        document.getElementById("userSpan").textContent = "El usuario debe ser de al menos 5 caracteres";
        //alert('El usuario debe ser de al menos 5 caracteres');
    }
    else {
        if (/[^a-zA-Z0-9\-\/]/.test(nombre.value)) {
            document.getElementById("userSpan").textContent = "El usuario solo puede tener alfanumericos";
            //  alert('El usuario solo puede tener alfanumericos')
        }
        else {
            var usuario = nombre.value
            $.ajax({
                cache: false,
                url: '/Usuario/validateUser',
                type: 'GET',
                data: { usr: usuario },
                success: function (result) {
                    if (result.success == true) {
                        document.getElementById("userSpan").textContent = "El usuario ya existe";
                        //            alert('El usuario ya existe')
                    }
                    else {
                        $('#newUserForm').submit()
                    }
                }
            })
        }

    }
}

function validateUserEdit() {    
    var nombre = document.getElementById('usrnm');
    if (document.getElementById('usrnm').value.length < 5) {
        document.getElementById("userSpan").textContent = "El usuario debe ser de al menos 5 caracteres";
    }
    else {
        if (/[^a-zA-Z0-9\-\/]/.test(nombre.value)) {
            document.getElementById("userSpan").textContent = "El usuario solo puede tener alfanumericos";
        }
        else {
            var usuario = nombre.value
            var idu = document.getElementById('idUsr').value
            $.ajax({
                cache: false,                
                url: '/Usuario/validateEditUser',
                type: 'GET',
                data: { usr: usuario, id: idu },
                success: function (result) {
                    if (result.success == true) {
                        document.getElementById("userSpan").textContent = "El usuario ya existe";
                    }
                    else {
                        $('#editUserForm').submit()
                    }
                }
            })
        }

    }
}

function validateUserProfile() {
    var nombre = document.getElementById('usrnm');
    if (document.getElementById('usrnm').value.length < 5) {
        document.getElementById("userSpan").textContent = "El usuario debe ser de al menos 5 caracteres";
    }
    else {
        if (/[^a-zA-Z0-9\-\/]/.test(nombre.value)) {
            document.getElementById("userSpan").textContent = "El usuario solo puede tener alfanumericos";
        }
        else {
            var usuario = nombre.value
            var idu = document.getElementById('idUsr').value
            $.ajax({
                cache: false,
                url: '../Usuario/validateEditUser',
                type: 'GET',
                data: { usr: usuario, id: idu },
                success: function (result) {
                    if (result.success == true) {
                        document.getElementById("userSpan").textContent = "El usuario ya existe";
                    }
                    else {
                        $('#editUserForm').submit()
                    }
                }
            })
        }

    }
}
