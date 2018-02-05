using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Sello.Api.Contracts;
using Sello.Tests.Integration.Services;

namespace Sello.Tests.Integration.Tests
{
    [Category("Integration")]
    public class ProductTests
    {
        [Test]
        [Category("Smoke")]
        public async Task Products_ListAllProducts_ShouldReturnHttpOk()
        {
            // Arrange
            const string productsUrl = ProductService.BaseUrl;
            var selloService = new SelloService();

            // Act
            var response = await selloService.GetResponseAsync(productsUrl);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var rawContent = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductInformationContract>>(rawContent);
            Assert.NotNull(products);
        }

        [Test]
        public async Task Products_ListAllProductsWithChaosEnabled_ShouldReturnHttpOk()
        {
            // Arrange
            const string productsUrl = ProductService.BaseUrl;
            var selloService = new SelloService(unleashChaosMonkeys: true);

            // Act
            var response = await selloService.GetResponseAsync(productsUrl);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Test]
        [Category("Smoke")]
        public async Task Products_GetProductDetailsForExistingProduct_ShouldReturnHttpOk()
        {
            // Arrange
            var selloService = new SelloService();
            var productService = new ProductService();
            var products = await productService.GetAllAsync();
            Assert.NotNull(products);
            Assert.IsNotEmpty(products);
            var firstProduct = products.First();

            // Act
            var response = await selloService.GetResponseAsync($"{ProductService.BaseUrl}/{firstProduct.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var rawContent = await response.Content.ReadAsStringAsync();
            var productInformation = JsonConvert.DeserializeObject<ProductInformationContract>(rawContent);
            Assert.AreEqual(firstProduct.Id, productInformation.Id);
            Assert.AreEqual(firstProduct.Name, productInformation.Name);
            Assert.AreEqual(firstProduct.Description, productInformation.Description);
            Assert.AreEqual(firstProduct.Price, productInformation.Price);
        }

        [Test]
        public async Task Products_GetProductDetailsForExistingProductWithChaosEnabled_ShouldReturnHttpInternalServerError()
        {
            // Arrange
            var selloService = new SelloService(unleashChaosMonkeys:true);
            var productService = new ProductService();
            var products = await productService.GetAllAsync();
            Assert.NotNull(products);
            Assert.IsNotEmpty(products);
            var firstProduct = products.First();

            // Act
            var response = await selloService.GetResponseAsync($"{ProductService.BaseUrl}/{firstProduct.Id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Test]
        public async Task Products_AddNewProductToCatalogWithChaosEnabled_ShouldReturnHttpInternalServerError()
        {
            // Arrange
            const string productsUrl = ProductService.BaseUrl;
            var selloService = new SelloService(unleashChaosMonkeys: true);
            var integrationTestId = Guid.NewGuid().ToString();
            var productName = $"Integration Product ({integrationTestId})";
            var productDescription = $"Product created by Integration Test - {integrationTestId}";
            const double productPrice = 100;

            var newProduct = new NewProductContract
            {
                Name = productName,
                Description = productDescription,
                Price = productPrice
            };

            // Act
            var response = await selloService.PostResponseAsync(productsUrl, newProduct);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Test]
        [Category("Smoke")]
        public async Task Products_GetProductDetailsForNotExistingProduct_ShouldReturnHttpNotFound()
        {
            // Arrange
            const string productId = "I-DO-NOT-EXIST";
            var selloService = new SelloService();

            // Act
            var response = await selloService.GetResponseAsync($"{ProductService.BaseUrl}/{productId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}