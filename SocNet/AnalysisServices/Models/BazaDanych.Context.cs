﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnalysisServices2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Networkv3Entities : DbContext
    {
        public Networkv3Entities()
            : base("name=Networkv3Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Credentials> Credentials { get; set; }
        public virtual DbSet<LinkDb> LinkDb { get; set; }
        public virtual DbSet<NetworkDb> NetworkDb { get; set; }
        public virtual DbSet<NetworkFactorsDb> NetworkFactorsDb { get; set; }
        public virtual DbSet<ServiceDb> ServiceDb { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<VertexDb> VertexDb { get; set; }
        public virtual DbSet<VertexFactorsDb> VertexFactorsDb { get; set; }
    }
}
