using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Threading.Tasks;
using Ninject;
using Sello.Common.DependencyInjection;
using Sello.Common.Security.Interfaces;
using Sello.Datastore.SQL.Model;

namespace Sello.Datastore.SQL
{
    public class PlatformDatabaseContext : DbContext
    {
        public PlatformDatabaseContext()
        {
        }

        private PlatformDatabaseContext(string connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        public static async Task<PlatformDatabaseContext> CreateAsync()
        {
            var secretProvider = PlatformKernel.Instance.Get<ISecretProvider>();
            var connectionString = await secretProvider
                .GetSecretAsync("Sql.Codito.ConnectionString");

            return new PlatformDatabaseContext(connectionString);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define default schema
            modelBuilder.HasDefaultSchema("platform");

            // Define table structure
            modelBuilder.Entity<Order>().HasKey(order => order.Id);
            modelBuilder.Entity<Order>().Property(order => order.ConfirmationId)
                .HasMaxLength(value: 600)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_ConfirmationId") {IsUnique = true}));

            modelBuilder.Entity<Product>().HasKey(product => product.Id);
            modelBuilder.Entity<Product>().Property(product => product.ExternalId)
                .HasMaxLength(value: 600)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_ExternalId") {IsUnique = true}));
            modelBuilder.Entity<Product>().Property(product => product.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(product => product.Price).IsRequired();

            modelBuilder.Entity<Customer>().HasKey(customer => customer.Id);
            modelBuilder.Entity<Customer>().Property(customer => customer.FirstName).IsRequired();
            modelBuilder.Entity<Customer>().Property(customer => customer.LastName).IsRequired();
            modelBuilder.Entity<Customer>().Property(customer => customer.EmailAddress)
                .HasMaxLength(value: 600)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_EmailAddress") {IsUnique = true}));
        }
    }
}