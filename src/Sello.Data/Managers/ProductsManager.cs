using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sello.Data.Repositories;
using Sello.Domain.Model;

namespace Sello.Data.Managers
{
    public class ProductsManager
    {
        private readonly ProductsRepository _productsRepository = new ProductsRepository();

        public async Task<List<Product>> GetAsync()
        {
            var products = await _productsRepository.GetAsync();
            return products.Select(MapDatabaseToDomain).ToList();
        }

        private static Product MapDatabaseToDomain(Datastore.SQL.Model.Product productRow)
        {
            var product = new Product
            {
                Name = productRow.Name,
                Description = productRow.Description,
                Price = productRow.Price
            };

            return product;
        }
    }
}