using System;
using System.Collections.Generic;
using System.Linq;
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
            var productId = Guid.NewGuid().ToString();
            const int orderItemAmount = 2;
            const double productPrice = 599;
            var customerContract = new CustomerContract
            {
                FirstName = customerFirstName,
                LastName = customerLastName,
                EmailAddress = customerEmailAddress
            };
            var orderItemContract = new OrderItemContract
            {
                Amount = orderItemAmount,
                Price = productPrice,
                ProductId = productId
            };

            var orderContract = new OrderContract
            {
                Customer = customerContract,
                TotalAmount = orderItemAmount * productPrice,
                Items = new List<OrderItemContract> { orderItemContract }
            };

            // Act
            var order = Mapper.Map<Order>(orderContract);

            // Assert
            Assert.NotNull(order);
            Assert.NotNull(order.Customer);
            Assert.NotNull(order.Items);
            Assert.AreEqual(customerFirstName, order.Customer.FirstName);
            Assert.AreEqual(customerLastName, order.Customer.LastName);
            Assert.AreEqual(customerEmailAddress, order.Customer.EmailAddress);
            var orderItem = order.Items.First();
            Assert.AreEqual(orderItemAmount, orderItem.Amount);
        }

        [Test]
        public void Product_MapFromContractToDbEntity_Succeeds()
        {
            // Arrange
            const string productName = "Xbox One X";
            const string productDescription = "Microsoft's latest gaming console";
            const double productPrice = 599;
            var productContract = new ProductContract
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
        public void Product_MapFromDbEntityToContract_Succeeds()
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
            var product = Mapper.Map<ProductContract>(databaseProduct);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productId, product.Id);
            Assert.AreEqual(productName, product.Name);
            Assert.AreEqual(productDescription, product.Description);
            Assert.AreEqual(productPrice, product.Price);
        }
    }
}