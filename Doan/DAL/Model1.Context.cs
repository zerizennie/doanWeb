﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Doan.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class huhuEntities : DbContext
    {
        public huhuEntities()
            : base("name=huhuEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bill> bills { get; set; }
        public virtual DbSet<catetory> catetories { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<order_detail_id> order_detail_id { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<shopping_cart> shopping_cart { get; set; }
    }
}
