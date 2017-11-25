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
        public void Product_MapFromDbEntityToContract_Succeeds()
        {
            // Arrange
            const string productName = "Xbox One X";
            const string productDescription = "Microsoft's latest gaming console";
            const double productPrice = 599;
            var databaseProduct = new Product
            {
                Name = productName,
                Description = productDescription,
                Price = productPrice
            };

            // Act
            var product = Mapper.Map<ProductContract>(databaseProduct);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productName, product.Name);
            Assert.AreEqual(productDescription, product.Description);
            Assert.AreEqual(productPrice, product.Price);
        }

        [Test]
        public void Product_MapFromContractToDbEntity_Succeeds()
        {
            // Arrange
            const string productName = "Xbox One X";
            const string productDescription = "Microsoft's latest gaming console";
            const double productPrice = 599;
            var databaseProduct = new ProductContract
            {
                Name = productName,
                Description = productDescription,
                Price = productPrice
            };

            // Act
            var product = Mapper.Map<Product>(databaseProduct);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productName, product.Name);
            Assert.AreEqual(productDescription, product.Description);
            Assert.AreEqual(productPrice, product.Price);
        }
    }
}