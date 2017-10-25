using System.Collections.Generic;

namespace Sello.Data.Repositories
{
    public class ProductsRepository
    {
        public List<string> Get()
        {
            return new List<string>
            {
                "Xbox One",
                "Surface Hub"
            };
        }
    }
}