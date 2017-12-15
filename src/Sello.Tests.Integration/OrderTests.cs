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
            const string customerLastName = "Kerkhove";
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

        [Test]
        public async Task Orders_CreateForExistingProductWithChangedPrice_ShouldReturnHttpBadRequest()
        {
            // Arrange
            const string ordersUrl = "order";
            const string customerFirstName = "Tom";
            const string customerLastName = "Kerkhove";
            const string customerEmailAddress = "Tom.Kerkhove@codit.eu";
            var productToBuy = await GetProductFromCatalogAsync();
            productToBuy.Price = productToBuy.Price * 2;

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
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Orders_CreateForExistingProductWithChangedDescription_ShouldReturnHttpBadRequest()
        {
            // Arrange
            const string ordersUrl = "order";
            const string customerFirstName = "Tom";
            const string customerLastName = "Kerkhove";
            const string customerEmailAddress = "Tom.Kerkhove@codit.eu";
            var productToBuy = await GetProductFromCatalogAsync();
            productToBuy.Description = "Altered description";

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
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Orders_CreateForExistingProductWithChangedName_ShouldReturnHttpBadRequest()
        {
            // Arrange
            const string ordersUrl = "order";
            const string customerFirstName = "Tom";
            const string customerLastName = "Kerkhove";
            const string customerEmailAddress = "Tom.Kerkhove@codit.eu";
            var productToBuy = await GetProductFromCatalogAsync();
            productToBuy.Name = "Altered name";

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
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Orders_CreateForExistingProductWithoutCustomerFirstName_ShouldReturnHttpBadRequest()
        {
            // Arrange
            const string ordersUrl = "order";
            var customerFirstName = string.Empty;
            const string customerLastName = "Kerkhove";
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
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Orders_CreateForExistingProductWithoutCustomerLastName_ShouldReturnHttpBadRequest()
        {
            // Arrange
            const string ordersUrl = "order";
            const string customerFirstName = "Tom";
            var customerLastName = string.Empty;
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
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Orders_CreateForExistingProductWithoutCustomerEmailAddress_ShouldReturnHttpBadRequest()
        {
            // Arrange
            const string ordersUrl = "order";
            const string customerFirstName = "Tom";
            const string customerLastName = "Kerkhove";
            var customerEmailAddress = string.Empty;
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
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Orders_CreateForNotExistingProduct_ShouldReturnHttpNotFound()
        {
            // Arrange
            const string ordersUrl = "order";
            const string productId = "I-DO-NOT-EXIST";
            const string productName = "Validation Product";
            const string productDescription = "Product created by Integration Test, however it should never make it in";
            const double productPrice = 100;
            const string customerFirstName = "Tom";
            const string customerLastName = "Tom";
            const string customerEmailAddress = "Tom.Kerkhove@codit.eu";

            var customer = new CustomerContract
            {
                FirstName = customerFirstName,
                LastName = customerLastName,
                EmailAddress = customerEmailAddress
            };
            var product = new ProductInformationContract
            {
                Id = productId,
                Name = productName,
                Description = productDescription,
                Price = productPrice
            };
            var order = new OrderContract
            {
                Customer = customer,
                Product = product
            };

            // Act
            var response = await _selloService.PostResponseAsync(ordersUrl, order);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
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