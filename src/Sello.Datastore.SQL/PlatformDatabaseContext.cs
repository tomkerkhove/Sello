using System.Data.Entity;
using Sello.Datastore.SQL.Model;

namespace Sello.Datastore.SQL
{
    public class PlatformDatabaseContext : DbContext
    {
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
            modelBuilder.Entity<Order>().Property(order => order.ConfirmationId).IsRequired();

            modelBuilder.Entity<Product>().HasKey(product => product.Id);
            modelBuilder.Entity<Product>().Property(product => product.ExternalId).IsRequired();
            modelBuilder.Entity<Product>().Property(product => product.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(product => product.Price).IsRequired();

            modelBuilder.Entity<Customer>().HasKey(customer => customer.Id);
            modelBuilder.Entity<Customer>().Property(customer => customer.FirstName).IsRequired();
            modelBuilder.Entity<Customer>().Property(customer => customer.LastName).IsRequired();
            modelBuilder.Entity<Customer>().Property(customer => customer.EmailAddress).IsRequired();
        }
    }
}