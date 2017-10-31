using System.Data.Entity.Migrations;
using System.Linq;
using Sello.Datastore.SQL.Model;

namespace Sello.Datastore.SQL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<PlatformDatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PlatformDatabaseContext context)
        {
            var xbox = new Product
            {
                Name = "Xbox One",
                Description = "Microsoft's latest gaming console",
                Price = 599
            };
            AddProductIfNotPresent(context, xbox);

            var surfaceHub = new Product
            {
                Name = "Surface Hub",
                Description = "Microsoft's latest collaboration tool",
                Price = 5999
            };
            AddProductIfNotPresent(context, surfaceHub);

            context.SaveChanges();
        }

        private static void AddProductIfNotPresent(PlatformDatabaseContext context, Product xbox)
        {
            if (context.Products.Any(product => product.Name.ToLower() == xbox.Name.ToLower()) == false)
                context.Products.Add(xbox);
        }
    }
}