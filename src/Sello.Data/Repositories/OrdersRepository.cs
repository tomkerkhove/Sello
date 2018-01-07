using System.Threading.Tasks;
using Sello.Datastore.SQL;
using Sello.Datastore.SQL.Model;

namespace Sello.Data.Repositories
{
    public class OrdersRepository
    {
        private readonly CustomersRepository _customersRepository;
        private readonly PlatformDatabaseContext _databaseContext;
        private readonly ProductsRepository _productsRepository;

        public OrdersRepository(ProductsRepository productsRepository, CustomersRepository customersRepository,
            PlatformDatabaseContext databaseContext)
        {
            _productsRepository = productsRepository;
            _customersRepository = customersRepository;
            _databaseContext = databaseContext;
        }

        public static async Task<OrdersRepository> CreateAsync()
        {
            var databaseContext = await PlatformDatabaseContext.CreateAsync();
            var productsRepository = new ProductsRepository(databaseContext);
            var customersRepository = new CustomersRepository(databaseContext);

            return new OrdersRepository(productsRepository, customersRepository, databaseContext);
        }

        /// <summary>
        ///     Adds a new order
        /// </summary>
        /// <param name="order">Product to add</param>
        public async Task<Order> AddAsync(Order order)
        {
            var foundCustomer = await _customersRepository.GetAsync(order.Customer.EmailAddress);
            if (foundCustomer != null)
            {
                order.Customer = null;
                order.CustomerId = foundCustomer.Id;
            }

            var foundProduct = await _productsRepository.GetAsync(order.Product.ExternalId);
            order.Product = null;
            order.ProductId = foundProduct.Id;

            var storedOrder = _databaseContext.Orders.Add(order);
            await _databaseContext.SaveChangesAsync();

            return storedOrder;
        }
    }
}