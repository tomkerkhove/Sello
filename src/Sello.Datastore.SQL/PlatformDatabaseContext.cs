using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Sello.Datastore.SQL.Model;
using Sello.Security;

namespace Sello.Datastore.SQL
{
    public class PlatformDatabaseContext : DbContext
    {
        private PlatformDatabaseContext(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
        }

        public static async Task<PlatformDatabaseContext> CreateAsync()
        {
            var keyVaultClient = new KeyVaultSecretProvider("XYZ");
            var connectionString = await keyVaultClient.GetSecretAsync("SQL-ConnectionString");
            return new PlatformDatabaseContext(connectionString);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define default schema
            modelBuilder.HasDefaultSchema("platform");

            // Define table structure
            modelBuilder.Entity<Order>().HasKey(order => order.Id);
            modelBuilder.Entity<Order>().Property(order => order.ConfirmationId)
                .HasMaxLength(600)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_ConfirmationId") { IsUnique = true }));

            modelBuilder.Entity<Product>().HasKey(product => product.Id);
            modelBuilder.Entity<Product>().Property(product => product.ExternalId)
                .HasMaxLength(600)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_ExternalId") { IsUnique = true }));
            modelBuilder.Entity<Product>().Property(product => product.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(product => product.Price).IsRequired();

            modelBuilder.Entity<Customer>().HasKey(customer => customer.Id);
            modelBuilder.Entity<Customer>().Property(customer => customer.FirstName).IsRequired();
            modelBuilder.Entity<Customer>().Property(customer => customer.LastName).IsRequired();
            modelBuilder.Entity<Customer>().Property(customer => customer.EmailAddress)
                .HasMaxLength(600)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_EmailAddress") { IsUnique = true }));
        }
    }
}