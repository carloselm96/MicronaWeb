
$('#modal-nuevo').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes
    var name = button.data('w2')

    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.       
    var modal = $(this)
    modal.find('.modal-body input#type').val(recipient)    
});

$('#editModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes
    var name = button.data('w2')
    var tipo= button.data('tipo')

    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.       
    var modal = $(this)
    modal.find('.modal-body input#idEd').val(recipient)    
    modal.find('.modal-body input#tipoEd').val(tipo)
    modal.find('.modal-body input#recipient-name').val(name)
});
$('#modal-delete').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes    
    var tipo = button.data('tipo')
    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.       
    var modal = $(this)
    modal.find('.modal-body input#idE').val(recipient)    
    modal.find('.modal-body input#typeE').val(tipo)
    //alert(recipient)
    //alert(tipo)

})

$('#eliminarForm').on('click', function () {
    $('#delete_form').submit();
});
$('#guardarForm').on('click', function () {
    $('#edit_form').submit();
});

$('#submitForm').on('click', function () {
    $('#new_form').submit();
});