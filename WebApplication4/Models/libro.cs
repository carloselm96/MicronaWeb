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
    
    public partial class libro
    {
        public int idLibro { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> Año { get; set; }
        public string ISBN { get; set; }
        public string Autores { get; set; }
        public Nullable<int> TipoLibro { get; set; }
        public Nullable<int> Usuario { get; set; }
        public Nullable<int> GrupoAcademico { get; set; }
    
        public virtual tipolibro tipolibro1 { get; set; }
        public virtual grupoacademico grupoacademico1 { get; set; }
        public virtual usuario usuario1 { get; set; }
    }
}
