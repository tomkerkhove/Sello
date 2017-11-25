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

        /// <summary>
        ///     Gets all products
        /// </summary>
        public async Task<List<Product>> GetAsync()
        {
            var products = await _databaseContext.Products.ToListAsync();
            return products;
        }

        /// <summary>
        ///     Gets a specific product
        /// </summary>
        /// <param name="productId">Id of the product</param>
        public async Task<Product> GetAsync(int productId)
        {
            var foundProduct = await _databaseContext.Products.SingleOrDefaultAsync(product => product.Id == productId);
            return foundProduct;
        }

        /// <summary>
        ///     Adds a new product
        /// </summary>
        /// <param name="product">Product to add</param>
        public async Task<Product> AddAsync(Product product)
        {
            var storedProduct = _databaseContext.Products.Add(product);
            await _databaseContext.SaveChangesAsync();

            return storedProduct;
        }
    }
}