//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class articulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public articulo()
        {
            this.articulo_grupo = new HashSet<articulo_grupo>();
            this.articulo_usuario = new HashSet<articulo_usuario>();
        }

        public int idArticulo { get; set; }
        [DisplayName("Titulo")]
        [Required(ErrorMessage = "Este Campo es Necesario")]
        public string Nombre { get; set; }
        public Nullable<int> Volumen { get; set; }
        [DisplayName("Página Inicio")]
        public Nullable<int> PagInicio { get; set; }
        [DisplayName("Página Final")]
        public Nullable<int> PagFinal { get; set; }
        [Required(ErrorMessage = "Este Campo es Necesario")]
        public string Revista { get; set; }
        public string ISSN { get; set; }
        [DisplayName("Fecha de Publicación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> Usuario { get; set; }
        public Nullable<int> Archivo { get; set; }
        public Nullable<int> TipoArticulo { get; set; }
        public string Indice { get; set; }
        public string DOI { get; set; }
        [DisplayName("Fecha de Aceptación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaAceptacion { get; set; }
        public virtual archivo archivo1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<articulo_grupo> articulo_grupo { get; set; }
        public virtual tipoarticulo tipoarticulo1 { get; set; }
        public virtual usuario usuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<articulo_usuario> articulo_usuario { get; set; }
    }
}
