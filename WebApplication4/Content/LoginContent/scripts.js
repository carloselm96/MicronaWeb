$(document).ready(function () {                
    $('#searchForm').on("submit", function (event) {
        event.preventDefault();
            var data1 = $("#searchForm").serialize();                
            $.ajax({                                        
                //type: "POST",
                url: "/Articulos/Busqueda",
                data: data1,                    
                success: function (articulos) {  
                    console.log(articulos);
                        /*$('#datat').DataTable( {
                        "ajax": articulos
                    } );*/
                }
            });
        });
    }); 