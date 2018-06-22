function crearCSV() {
    var tablehtml = document.getElementById('datat').innerHTML;
    var datos = tablehtml.replace(/<thead>/g, '')
        .replace(/<\/thead>/g, '')
        .replace(/<tbody>/g, '')
        .replace(/<\/tbody>/g, '')
        .replace(/<tr>/g, '')
        .replace(/<\/tr/g, '\r\n')
        .replace(/<td>/g, '')
        .replace(/<\/td>/g, ';')
        .replace(/\t/g, '')
        .replace(/\n/g, ';');
    alert(datos);
}