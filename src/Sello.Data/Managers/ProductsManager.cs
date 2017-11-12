using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            return products.Select(Mapper.Map<Product>).ToList();
        }
    }
}