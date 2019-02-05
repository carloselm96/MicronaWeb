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

    public partial class libro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public libro()
        {
            this.libro_grupo = new HashSet<libro_grupo>();
            this.libro_usuario = new HashSet<libro_usuario>();
        }
    
        public int idLibro { get; set; }
        [DisplayName("Titulo")]
        [Required(ErrorMessage = "Este Campo es Necesario")]
        public string Nombre { get; set; }
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Año { get; set; }
        public string ISBN { get; set; }
        public Nullable<int> TipoLibro { get; set; }
        public Nullable<int> Usuario { get; set; }
        public Nullable<int> Archivo { get; set; }
        public string Editorial { get; set; }
        public string Pais { get; set; }
    
        public virtual archivo archivo1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<libro_grupo> libro_grupo { get; set; }
        public virtual tipolibro tipolibro1 { get; set; }
        public virtual usuario usuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<libro_usuario> libro_usuario { get; set; }
    }
}
