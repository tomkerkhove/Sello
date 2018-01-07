using System.Data.Entity;
using System.Threading.Tasks;
using Sello.Datastore.SQL;
using Sello.Datastore.SQL.Model;

namespace Sello.Data.Repositories
{
    public class CustomersRepository
    {
        private readonly PlatformDatabaseContext _databaseContext;

        public CustomersRepository(PlatformDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public static async Task<CustomersRepository> CreateAsync()
        {
            var databaseContext = await PlatformDatabaseContext.CreateAsync();

            return new CustomersRepository(databaseContext);
        }

        /// <summary>
        ///     Gets a specific customer
        /// </summary>
        /// <param name="emailAddress">Email address of the customer</param>
        public async Task<Customer> GetAsync(string emailAddress)
        {
            var foundCustomer = await _databaseContext.Customers
                .SingleOrDefaultAsync(customer => customer.EmailAddress.ToLower() == emailAddress.ToLower());
            return foundCustomer;
        }
    }
}