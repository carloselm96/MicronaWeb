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

    public partial class trabajo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public trabajo()
        {
            this.trabajo_grupo = new HashSet<trabajo_grupo>();
            this.trabajo_usuario = new HashSet<trabajo_usuario>();
        }
    
        public int idTrabajo { get; set; }
        [Required(ErrorMessage = "Este Campo es Necesario")]
        public string Nombre { get; set; }
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Año { get; set; }
        [DisplayName("Tipo de Trabajo")]       
        public Nullable<int> TipoTrabajo { get; set; }
        public string Autores { get; set; }
        public string Presentacion { get; set; }
        public string Pais { get; set; }
        public Nullable<int> Usuario { get; set; }
        public Nullable<int> Archivo { get; set; }
    
        public virtual archivo archivo1 { get; set; }
        public virtual tipotrabajo tipotrabajo1 { get; set; }
        public virtual usuario usuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<trabajo_grupo> trabajo_grupo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<trabajo_usuario> trabajo_usuario { get; set; }
    }
}
