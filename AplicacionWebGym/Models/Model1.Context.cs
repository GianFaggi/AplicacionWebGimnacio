﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AplicacionWebGym.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AppGym2Entities : DbContext
    {
        public AppGym2Entities()
            : base("name=AppGym2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DatosPersona> DatosPersona { get; set; }
        public virtual DbSet<DetalleEjerciciosP> DetalleEjerciciosP { get; set; }
        public virtual DbSet<DetalleInventario> DetalleInventario { get; set; }
        public virtual DbSet<Ejercicios_P> Ejercicios_P { get; set; }
        public virtual DbSet<Ejercicios_T> Ejercicios_T { get; set; }
        public virtual DbSet<login_DB> login_DB { get; set; }
        public virtual DbSet<Medidas> Medidas { get; set; }
        public virtual DbSet<Pagos> Pagos { get; set; }
        public virtual DbSet<Problemas> Problemas { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Inventario> Inventario { get; set; }
        public virtual DbSet<Turnos> Turnos { get; set; }
        public virtual DbSet<DetalleEjercicioT> DetalleEjercicioT { get; set; }
    }
}
