$(function () {
    function callView(controller, ActionName) {            
        $.ajax({
            type: 'POST',
            url: '@Url.Content("~/'+controller+'/'+ActionName+'")',
            data: objectToPass,
            success: function (data) {
                $('#divid').innerHTML = data;
            }
        }
    }
});