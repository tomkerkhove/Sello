using AutoMapper;
using NUnit.Framework;

// ReSharper disable RedundantNameQualifier
namespace Sello.Tests.Unit.Mappings
{
    [Category("Unit")]
    public class DomainMappingTests
    {
        [Test]
        public void Product_MapFromProductDbEntity_Succeeds()
        {
            // Arrange
            const string productName = "Xbox One X";
            const string productDescription = "Microsoft's latest gaming console";
            const double productPrice = 599;
            var databaseProduct = new Sello.Datastore.SQL.Model.Product
            {
                Name = productName,
                Description = productDescription,
                Price = productPrice
            };

            // Act
            var product = Mapper.Map<Sello.Domain.Model.Product>(databaseProduct);

            // Assert
            Assert.NotNull(product);
            Assert.AreEqual(productName, product.Name);
            Assert.AreEqual(productDescription, product.Description);
            Assert.AreEqual(productPrice, product.Price);
        }
    }
}
