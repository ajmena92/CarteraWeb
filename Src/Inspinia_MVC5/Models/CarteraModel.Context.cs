﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebCartera.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarteraEntities : DbContext
    {
        public CarteraEntities()
            : base("name=CarteraEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<seguridadrolmodulo> seguridadrolmoduloes { get; set; }
        public virtual DbSet<seguridadusuario> seguridadusuarios { get; set; }
        public virtual DbSet<tmovimiento> tmovimientoes { get; set; }
        public virtual DbSet<tparametro> tparametros { get; set; }
        public virtual DbSet<seguridadmodulo> seguridadmoduloes { get; set; }
        public virtual DbSet<seguridadrol> seguridadrols { get; set; }
        public virtual DbSet<tcategoria> tcategorias { get; set; }
        public virtual DbSet<tmoneda> tmonedas { get; set; }
        public virtual DbSet<tsaldoxcategoria> tsaldoxcategorias { get; set; }
        public virtual DbSet<tcuenta> tcuentas { get; set; }
    }
}
