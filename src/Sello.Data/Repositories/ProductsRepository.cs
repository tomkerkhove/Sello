using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Sello.Datastore.SQL;
using Sello.Datastore.SQL.Model;

namespace Sello.Data.Repositories
{
    public class ProductsRepository
    {
        private readonly PlatformDatabaseContext _databaseContext = new PlatformDatabaseContext();

        public async Task<List<Product>> GetAsync()
        {
            var products = await _databaseContext.Products.ToListAsync();
            return products;
        }
    }
}