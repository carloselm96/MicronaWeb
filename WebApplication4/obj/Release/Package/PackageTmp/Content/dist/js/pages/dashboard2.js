$(function () {
    
    $('.select2').select2();    
    $(".swalalert").click(function (e) {
        e.preventDefault();
        // stop the default behavior(navigation)
        var _form = $(this).closest("form");
        //get a reference to the container form 

        swal({
            title: "Esta seguro?",
            text: "Una vez elimnado no se podra recuperar este archivo",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {                
                _form.submit();                
            } else {
                swal("Se ha cancelado la accion");
            }
        });
    })
})