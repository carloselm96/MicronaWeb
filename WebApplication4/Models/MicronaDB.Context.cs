﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class microna2018Entities : DbContext
    {
        public microna2018Entities()
            : base("name=microna2018Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<archivo> archivo { get; set; }
        public virtual DbSet<articulo> articulo { get; set; }
        public virtual DbSet<articulo_grupo> articulo_grupo { get; set; }
        public virtual DbSet<capitulo_grupo> capitulo_grupo { get; set; }
        public virtual DbSet<capitulolibro> capitulolibro { get; set; }
        public virtual DbSet<grupoacademico> grupoacademico { get; set; }
        public virtual DbSet<libro> libro { get; set; }
        public virtual DbSet<libro_grupo> libro_grupo { get; set; }
        public virtual DbSet<proyecto_grupo> proyecto_grupo { get; set; }
        public virtual DbSet<proyectos> proyectos { get; set; }
        public virtual DbSet<tipoarticulo> tipoarticulo { get; set; }
        public virtual DbSet<tipolibro> tipolibro { get; set; }
        public virtual DbSet<tipotrabajo> tipotrabajo { get; set; }
        public virtual DbSet<tipousuario> tipousuario { get; set; }
        public virtual DbSet<trabajo> trabajo { get; set; }
        public virtual DbSet<trabajo_grupo> trabajo_grupo { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
    }
}
