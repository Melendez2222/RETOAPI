using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RETOAPI.Models;
using System.Data;

namespace RETOAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Rols> Rols { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoicesDetail { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Rols>().ToTable("Rols");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<CategoryProduct>().ToTable("CategoryProduct");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Invoice>().ToTable("Invoice");
            modelBuilder.Entity<InvoiceDetail>().ToTable("InvoiceDetail");

            
            modelBuilder.HasSequence<int>("InvoiceNumberSequence")
            .StartsAt(1)
            .IncrementsBy(1);
            modelBuilder.Entity<Invoice>()
                .Property(f => f.InvoiceNumber)
                .HasDefaultValueSql("NEXT VALUE FOR InvoiceNumberSequence");

        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Invoice>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.IGV).CurrentValue = entry.Entity.Subtotal * entry.Entity.PercentageIGV / 100;
                    entry.Property(e => e.Total).CurrentValue = entry.Entity.Subtotal + entry.Property(e => e.IGV).CurrentValue;
                }
            }
            foreach (var entry in ChangeTracker.Entries<InvoiceDetail>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.SubtotalProduc).CurrentValue = entry.Entity.ProductPrice * entry.Entity.Quantity;
                    var product = Products.Find(entry.Entity.ProductID);
                    if (product != null)
                    {
                        product.Stock -= entry.Entity.Quantity;
                        Products.Update(product);
                    }
                }
            }
            return base.SaveChanges();
        }
        
    }
}
