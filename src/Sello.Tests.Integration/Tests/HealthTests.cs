using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Sello.Tests.Integration.Services;

namespace Sello.Tests.Integration.Tests
{
    [Category("Integration")]
    public class HealthTests
    {
        private readonly SelloService _selloService = new SelloService();

        [Test]
        [Category("Smoke")]
        public async Task Health_ApiShouldBeHealthy_ShouldReturnHttpOk()
        {
            // Arrange
            const string healthUri = "api/v1/health";

            // Act
            var response = await _selloService.GetResponseAsync(healthUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}