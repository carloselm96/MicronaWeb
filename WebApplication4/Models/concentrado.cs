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

    public partial class concentrado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public concentrado()
        {
            this.concentrado_autores = new HashSet<concentrado_autores>();
            this.concentrado_grupos = new HashSet<concentrado_grupos>();
        }
    
        public int idConcentrado { get; set; }
        public string Titulo { get; set; }
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> TipoConcentrado { get; set; }
        public Nullable<int> IdItem { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<concentrado_autores> concentrado_autores { get; set; }
        public virtual tipoconcentrado tipoconcentrado1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<concentrado_grupos> concentrado_grupos { get; set; }
    }
}
