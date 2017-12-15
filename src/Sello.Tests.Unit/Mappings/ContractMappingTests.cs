using System;
using AutoMapper;
using NUnit.Framework;
using Sello.Api.Contracts;
using Sello.Datastore.SQL.Model;

// ReSharper disable RedundantNameQualifier
namespace Sello.Tests.Unit.Mappings
{
    [Category("Unit")]
    public class ContractMappingTests : MappingTest
    {
        [Test]
        public void Customer_MapFromContractToDbEntity_Succeeds()
        {
            // Arrange
            const string customerFirstName = "John";
            const string customerLastName = "Doe";
            const string customerEmailAddress = "john.doe@sello.io";
            var customerContract = new CustomerContract
            {
                FirstName = customerFirstName,
                LastName = customerLastName,
                EmailAddress = customerEmailAddress
            };

            // Act
            var customer = Mapper.Map<Customer>(customerContract);

            // Assert
            Assert.NotNull(customer);
            Assert.AreEqual(customerFirstName, customer.FirstName);
            Assert.AreEqual(customerLastName, customer.LastName);
            Assert.AreEqual(customerEmailAddress, customer.EmailAddress);
        }

        [Test]
        public void Order_MapFromContractToDbEntity_Succeeds()
        {
            // Arrange
            const string customerFirstName = "John";
            const string customerLastName = "Doe";
            const string customerEmailAddress = "john.doe@sello.io";
            const string productName = "Xbox One X";
            const string productDescription = "Microsoft's latest gaming console";
            const double productPrice = 599;
            string productId = Guid.NewGuid().ToString();
            var customerContract = new CustomerContract
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

            var orderContract = new OrderContract
            {
                Customer = customerContract,
                Product = product
            };

            // Act
            var order = Mapper.Map<Order>(orderContract);

            // Assert
            Assert.NotNull(order);
            Assert.NotNull(order.Customer);
            Assert.AreEqual(customerFirstName, order.Customer.FirstName);
            Assert.AreEqual(customerLastName, order.Customer.LastName);
            Assert.AreEqual(customerEmailAddress, order.Customer.EmailAddress);
            Assert.NotNull(order.Product);
            Assert.AreEqual(productId, order.Product.ExternalId);
            Assert.AreEqual(productName, order.Product.Name);
            Assert.AreEqual(productPrice, order.Product.Price);
            Assert.AreEqual(productDescription, order.Product.Description);
        }

        [Test]
        public void Product_MapFromProductInformationContractToDbEntity_Succeeds()
        {
            // Arrange
            const string productName = "Xbox One X";
            const string productDescription = "Microsoft's latest gaming console";
            const double productPrice = 599;
            string productId = Guid.NewGuid().ToString();
            var productContract = new ProductInformationContract
            {
                Id= productId,
                Name = productName,
                Description = productDescription,
                Price = productPrice
            };

            // Act
            var product = Mapper.Map<Product>(productContract);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productId, product.ExternalId);
            Assert.AreEqual(productName, product.Name);
            Assert.AreEqual(productDescription, product.Description);
            Assert.AreEqual(productPrice, product.Price);
        }

        [Test]
        public void Product_MapFromNewProductContractToDbEntity_Succeeds()
        {
            // Arrange
            const string productName = "Xbox One X";
            const string productDescription = "Microsoft's latest gaming console";
            const double productPrice = 599;
            var productContract = new NewProductContract
            {
                Name = productName,
                Description = productDescription,
                Price = productPrice
            };

            // Act
            var product = Mapper.Map<Product>(productContract);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productName, product.Name);
            Assert.AreEqual(productDescription, product.Description);
            Assert.AreEqual(productPrice, product.Price);
        }

        [Test]
        public void Product_MapFromDbEntityToProductInformationContract_Succeeds()
        {
            // Arrange
            const string productName = "Xbox One X";
            const string productDescription = "Microsoft's latest gaming console";
            const double productPrice = 599;
            var productId = Guid.NewGuid().ToString();
            var databaseProduct = new Product
            {
                ExternalId = productId,
                Name = productName,
                Description = productDescription,
                Price = productPrice
            };

            // Act
            var product = Mapper.Map<ProductInformationContract>(databaseProduct);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productId, product.Id);
            Assert.AreEqual(productName, product.Name);
            Assert.AreEqual(productDescription, product.Description);
            Assert.AreEqual(productPrice, product.Price);
        }
    }
}