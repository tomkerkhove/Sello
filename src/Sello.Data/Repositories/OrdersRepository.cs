using System.Threading.Tasks;
using Sello.Datastore.SQL;
using Sello.Datastore.SQL.Model;

namespace Sello.Data.Repositories
{
    public class OrdersRepository
    {
        private readonly CustomersRepository _customersRepository = new CustomersRepository();
        private readonly PlatformDatabaseContext _databaseContext = new PlatformDatabaseContext();
        private readonly ProductsRepository _productsRepository = new ProductsRepository();

        /// <summary>
        ///     Adds a new order
        /// </summary>
        /// <param name="order">Product to add</param>
        public async Task<Order> AddAsync(Order order)
        {
            var foundCustomer = await _customersRepository.GetAsync(order.Customer.EmailAddress);
            order.Customer = null;
            order.CustomerId = foundCustomer.Id;

            var foundProduct = await _productsRepository.GetAsync(order.Product.ExternalId);
            order.Product = null;
            order.ProductId = foundProduct.Id;

            var storedOrder = _databaseContext.Orders.Add(order);
            await _databaseContext.SaveChangesAsync();

            return storedOrder;
        }
    }
}