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
    
    public partial class articulo_grupo
    {
        public int idarticulo_grupo { get; set; }
        public Nullable<int> id_articulo { get; set; }
        public Nullable<int> id_grupo { get; set; }
    
        public virtual articulo articulo { get; set; }
        public virtual grupoacademico grupoacademico { get; set; }
    }
}
