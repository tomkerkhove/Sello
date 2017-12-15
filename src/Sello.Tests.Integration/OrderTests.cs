using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Sello.Api.Contracts;
using Sello.Tests.Integration.Services;

namespace Sello.Tests.Integration
{
    [Category("Integration")]
    public class OrderTests
    {
        private readonly SelloService _selloService = new SelloService();

        [Test]
        public async Task Orders_CreateForExistingProduct_ShouldReturnHttpOk()
        {
            // Arrange
            const string ordersUrl = "order";
            const string customerFirstName = "Tom";
            const string customerLastName = "Tom";
            const string customerEmailAddress = "Tom.Kerkhove@codit.eu";
            var productToBuy = await GetProductFromCatalogAsync();

            var customer = new CustomerContract
            {
                FirstName = customerFirstName,
                LastName = customerLastName,
                EmailAddress = customerEmailAddress
            };
            var order = new OrderContract
            {
                Customer = customer,
                Product = productToBuy
            };

            // Act
            var response = await _selloService.PostResponseAsync(ordersUrl, order);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var rawContent = await response.Content.ReadAsStringAsync();
            var orderConfirmation = JsonConvert.DeserializeObject<OrderConfirmationContract>(rawContent);
            Assert.NotNull(orderConfirmation);
            Assert.NotNull(orderConfirmation.ConfirmationId);
            Assert.IsNotEmpty(orderConfirmation.ConfirmationId);
            Assert.NotNull(orderConfirmation.Order);
            Assert.NotNull(orderConfirmation.Order.Product);
            Assert.AreEqual(productToBuy.Id, orderConfirmation.Order.Product.Id);
            Assert.AreEqual(productToBuy.Name, orderConfirmation.Order.Product.Name);
            Assert.AreEqual(productToBuy.Description, orderConfirmation.Order.Product.Description);
            Assert.AreEqual(productToBuy.Price, orderConfirmation.Order.Product.Price);
            Assert.NotNull(orderConfirmation.Order.Customer);
            Assert.AreEqual(customerFirstName, orderConfirmation.Order.Customer.FirstName);
            Assert.AreEqual(customerLastName, orderConfirmation.Order.Customer.LastName);
            Assert.AreEqual(customerEmailAddress, orderConfirmation.Order.Customer.EmailAddress);
        }

        private static async Task<ProductInformationContract> GetProductFromCatalogAsync()
        {
            var productService = new ProductService();
            var products = await productService.GetAllAsync();
            Assert.NotNull(products);
            Assert.IsNotEmpty(products);
            var productToBuy = products.First();
            return productToBuy;
        }
    }
}