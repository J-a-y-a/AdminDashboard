﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminDashboard
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class jayaEntities : DbContext
    {
        public jayaEntities()
            : base("name=jayaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ENQUIRY_INFO> ENQUIRY_INFO { get; set; }
        public virtual DbSet<TELE_DESIGNATION_MASTER> TELE_DESIGNATION_MASTER { get; set; }
        public virtual DbSet<MEDIAMASTER> MEDIAMASTERs { get; set; }
        public virtual DbSet<TELE_ENQUIRY_INFO> TELE_ENQUIRY_INFO { get; set; }
    }
}
