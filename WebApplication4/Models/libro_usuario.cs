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
    
    public partial class libro_usuario
    {
        public int idLibro_Usuario { get; set; }
        public Nullable<int> Id_Libro { get; set; }
        public Nullable<int> Id_Usuario { get; set; }
    
        public virtual libro libro { get; set; }
        public virtual usuario usuario { get; set; }
    }
}
