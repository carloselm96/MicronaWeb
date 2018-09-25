



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

function searchConcentrado() {
    var dataform = $("#searchForm").serialize();
    $.ajax({
        type: "POST",
        url: "/Home/Search",
        data: dataform,
        success: function (data) {
            var table = $("#datat").DataTable({
                data: data.data,
                destroy: true,
            })
        }
    })
}

