﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NGO_Project
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NGOEntities1 : DbContext
    {
        public NGOEntities1()
            : base("name=NGOEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AidRequest> AidRequests { get; set; }
        public virtual DbSet<CashDonation> CashDonations { get; set; }
        public virtual DbSet<DistributionOrderItem> DistributionOrderItems { get; set; }
        public virtual DbSet<DistributionOrder> DistributionOrders { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DonationItem> DonationItems { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<Donor> Donors { get; set; }
        public virtual DbSet<InventoryItem> InventoryItems { get; set; }
        public virtual DbSet<RequestedItem> RequestedItems { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
    }
}
