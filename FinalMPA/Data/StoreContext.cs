using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalMPA.Models;

namespace FinalMPA.Data
{
    public class StoreContext:DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<PublishedMagazine> PublishedMagazines { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Magazine>().ToTable("Magazine");
            modelBuilder.Entity<Publisher>().ToTable("Publisher");
            modelBuilder.Entity<PublishedMagazine>().ToTable("PublishedMagazine");
            modelBuilder.Entity<PublishedMagazine>().HasKey(c => new { c.MagazineID, c.PublisherID });
        }
    }
}
