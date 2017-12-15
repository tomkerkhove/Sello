using System.Data.Entity;
using System.Security.Permissions;
using System.Threading.Tasks;
using Sello.Datastore.SQL;
using Sello.Datastore.SQL.Model;

namespace Sello.Data.Repositories
{
    public class OrdersRepository
    {
        private readonly PlatformDatabaseContext _databaseContext = new PlatformDatabaseContext();

        /// <summary>
        ///     Adds a new order
        /// </summary>
        /// <param name="order">Product to add</param>
        public async Task<Order> AddAsync(Order order)
        {
            var storedOrder = _databaseContext.Orders.Add(order);

            await _databaseContext.SaveChangesAsync();

            return storedOrder;
        }
    }
}