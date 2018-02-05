using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Sello.Tests.Integration.Services;

namespace Sello.Tests.Integration.Tests
{
    [Category("Integration")]
    public class HealthTests
    {
        [Test]
        public async Task Health_ApiShouldBeHealthy_ShouldReturnHttpOk()
        {
            // Arrange
            const string healthUri = "api/v1/health";
            var selloService = new SelloService();

            // Act
            var response = await selloService.GetResponseAsync(healthUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task Health_ApiIsUnhealthyBecauseOfMonkeys_ShouldReturnHttpInternalServerError()
        {
            // Arrange
            const string healthUri = "api/v1/health";
            var selloService = new SelloService(unleashChaosMonkeys:true);

            // Act
            var response = await selloService.GetResponseAsync(healthUri);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}