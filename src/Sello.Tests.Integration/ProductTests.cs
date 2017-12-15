using System;
using System.Collections.Generic;
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
    public class ProductTests
    {
        private readonly SelloService _selloService = new SelloService();

        [Test]
        [Category("Smoke")]
        public async Task Products_ListAllProducts_ShouldReturnHttpOk()
        {
            // Arrange
            const string productsUrl = ProductService.BaseUrl;

            // Act
            var response = await _selloService.GetResponseAsync(productsUrl);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var rawContent = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductInformationContract>>(rawContent);
            Assert.NotNull(products);
        }

        [Test]
        [Category("Smoke")]
        public async Task Products_GetProductDetailsForExistingProduct_ShouldReturnHttpOk()
        {
            // Arrange
            var productService = new ProductService();
            var products = await productService.GetAllAsync();
            Assert.NotNull(products);
            Assert.IsNotEmpty(products);
            var firstProduct = products.First();

            // Act
            var response = await _selloService.GetResponseAsync($"{ProductService.BaseUrl}/{firstProduct.Id}");

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
        public async Task Products_AddNewProductToCatalog_ShouldReturnHttpOk()
        {
            // Arrange
            const string productsUrl = ProductService.BaseUrl;
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
            var response = await _selloService.PostResponseAsync(productsUrl, newProduct);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var rawContent = await response.Content.ReadAsStringAsync();
            var productInformation = JsonConvert.DeserializeObject<ProductInformationContract>(rawContent);
            Assert.NotNull(productInformation);
            Assert.NotNull(productInformation.Id);
            Assert.IsNotEmpty(productInformation.Id);
            Assert.AreEqual(productName, productInformation.Name);
            Assert.AreEqual(productDescription, productInformation.Description);
            Assert.AreEqual(productPrice, productInformation.Price);
        }

        [Test]
        [Category("Smoke")]
        public async Task Products_GetProductDetailsForNotExistingProduct_ShouldReturnHttpNotFound()
        {
            // Arrange
            const string productId = "I-DO-NOT-EXIST";

            // Act
            var response = await _selloService.GetResponseAsync($"{ProductService.BaseUrl}/{productId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}