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
    
    public partial class docencia
    {
        public int idDocencia { get; set; }
        public string Titulo { get; set; }
        public Nullable<int> Autor { get; set; }
        public string Dependencia { get; set; }
        public string ProgramaEdu { get; set; }
        public string Nivel { get; set; }
        public System.DateTime FechaIni { get; set; }
        public int Alumnos { get; set; }
        public int Duracion { get; set; }
        public int Asesoria { get; set; }
        public int HorasSemana { get; set; }
        public Nullable<sbyte> ConsiderarCurr { get; set; }
        public string Coautores { get; set; }
        public Nullable<int> FK_Archivo { get; set; }
    
        public virtual archivo archivo { get; set; }
        public virtual usuario usuario { get; set; }
    }
}
