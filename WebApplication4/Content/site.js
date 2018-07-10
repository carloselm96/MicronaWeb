var msg = "Prueba";
$(function validateMyForm() {
    var usr = document.getElementById('usrnm');
    $.ajax({
        type: "POST",
        traditional: true,
        async: false,
        cache: false,
        url: '/validateUserName',
        context: document.body,
        data: usr,
        success: function (result) {
            if (result == "True") {
                return True;
            }
            else {
                alert("El usuario ya existe");
                return False;
            }
        },
        error: function (xhr) {
            //debugger;  
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });  
});