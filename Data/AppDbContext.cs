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

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
                entity.Property(e => e.UserActive).HasDefaultValue(true);
                entity.Property(e => e.Attemp).HasDefaultValue(0);
                entity.Property(e => e.UserUsername).HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.UserPassword).HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<Rols>(entity =>
            {
                entity.HasKey(e => e.RolId);
                entity.Property(e => e.RolId).ValueGeneratedOnAdd();
                entity.Property(e => e.RolActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RolId });

                entity.HasOne(ur => ur.Users)
                      .WithMany(u => u.UserRols)
                      .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Rols)
                      .WithMany(r => r.UserRols)
                      .HasForeignKey(ur => ur.RolId);
                entity.Property(ur => ur.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id_Product);
                entity.Property(e => e.Id_Product).ValueGeneratedOnAdd();
                entity.Property(e => e.ProductCode).IsRequired().HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.ProductName).IsRequired().HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.CatProductId).IsRequired();
                entity.HasOne(e => e.CategoryProduct)
                      .WithMany()
                      .HasForeignKey(e => e.CatProductId);
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Stock).IsRequired();
                entity.Property(e => e.ProductActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasKey(e => e.CatProductId);
                entity.Property(e => e.CatProductId).ValueGeneratedOnAdd();
                entity.Property(e => e.CatProductName).IsRequired().HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.CatProductActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceId);
                entity.Property(e => e.InvoiceId).ValueGeneratedOnAdd();
                entity.Property(e => e.InvoiceNumber).IsRequired();
                entity.Property(e => e.ClientID).IsRequired().HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.EmployeeID).IsRequired().HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.Subtotal).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.PercentageIGV).IsRequired().HasColumnType("decimal(18,2)");
                entity.Ignore(e => e.IGV);
                entity.Ignore(e => e.Total);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.ItemId);
                entity.Property(e => e.ItemId).ValueGeneratedOnAdd();
                entity.Property(e => e.InvoiceID).IsRequired();
                entity.HasOne(e => e.Invoice)
                      .WithMany()
                      .HasForeignKey(e => e.InvoiceID);
                entity.Property(e => e.ProductID).IsRequired();
                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductID);
                entity.Property(e => e.ProductName).IsRequired().HasColumnType("nvarchar(MAX)");
                entity.Property(e => e.ProductPrice).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.SubtotalProduc).IsRequired().HasColumnType("decimal(18,2)");
            });

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
