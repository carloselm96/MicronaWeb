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
    
    public partial class memorias
    {
        public int idMemorias { get; set; }
        public Nullable<int> Autor { get; set; }
        public string Titulo { get; set; }
        public string Congreso { get; set; }
        public int PaginaIni { get; set; }
        public int PaginaFin { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Ciudad { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Proposito { get; set; }
        public Nullable<sbyte> ConsiderarCurr { get; set; }
        public string Coautores { get; set; }
        public Nullable<int> FK_Archivo { get; set; }
    
        public virtual archivo archivo { get; set; }
        public virtual usuario usuario { get; set; }
    }
}
