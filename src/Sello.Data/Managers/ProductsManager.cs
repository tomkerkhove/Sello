using System.Collections.Generic;
using Sello.Data.Repositories;

namespace Sello.Data.Managers
{
    public class ProductsManager
    {
        private readonly ProductsRepository _productsRepository = new ProductsRepository();

        public List<string> Get()
        {
            return _productsRepository.Get();
        }
    }
}