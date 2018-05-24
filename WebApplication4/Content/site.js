$(document).ready(function () {
    // Setup - add a text input to each footer cell
    var art = $('#data_art').DataTable();
    var trab = $('#data_trab').DataTable();
    $('#nombreSearch').on('keyup change', function () {
        art.column(0).search($(this).val()).draw();
    });
    $('#autorSearch').on('keyup change', function () {
        art.column(1).search($(this).val()).draw();
    });
    $('#revistaSearch').on('keyup change', function () {
        art.column(4).search($(this).val()).draw();
    });
    $('#dateSearch').on('keyup change', function () {
        art.column(4).search($(this).val()).draw();
    });
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var min = parseInt($('#min').val(), 10);
            var max = parseInt($('#max').val(), 10);
            var age = parseFloat(data[6]) || 0; // use data for the age column

            if ((isNaN(min) && isNaN(max)) ||
                (isNaN(min) && age <= max) ||
                (min <= age && isNaN(max)) ||
                (min <= age && age <= max)) {
                return true;
            }
            return false;
        }
    );
    $('#min, #max').on('keyup change', function() {
        art.draw();
    });    
});