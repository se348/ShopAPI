using Microsoft.EntityFrameworkCore;
using Shop.DAL.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DAL.Data
{
    public class ShopContext: DbContext
    {
        public ShopContext() { 
        }

        public ShopContext(DbContextOptions<ShopContext> options): base(options) 
        {
        }

        public DbSet<Customer> Customers { get; set; } 
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ShopAPI;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable(nameof(Customer));
            modelBuilder.Entity<Order>().ToTable(nameof(Order));
            modelBuilder.Entity<Product>().ToTable(nameof(Product));
            modelBuilder.Entity<ProductOrder>().ToTable(nameof(ProductOrder));
            modelBuilder.Entity<User>().ToTable(nameof(User));  
        }

    }
}
