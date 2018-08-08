$('#editModalArt').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes
    var name = button.data('w2')

    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.       
    var modal = $(this)
    modal.find('.modal-body input#idArt').val(recipient)
    modal.find('.modal-body input#recipient-name').val(name)
});
$('#model-deleteart').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes    

    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.       
    var modal = $(this)
    modal.find('.modal-body input#idDArt').val(recipient)    
})
$('#editModalTrab').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes
    var name = button.data('w2')

    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.       
    var modal = $(this)
    modal.find('.modal-body input#idTrab').val(recipient)
    modal.find('.modal-body input#recipient-name').val(name)
})
$('#editModalLib').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget) // Button that triggered the modal
    var recipient = button.data('whatever') // Extract info from data-* attributes
    var name = button.data('w2')

    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.       
    var modal = $(this)
    modal.find('.modal-body input#idLib').val(recipient)
    modal.find('.modal-body input#recipient-name').val(name)
})