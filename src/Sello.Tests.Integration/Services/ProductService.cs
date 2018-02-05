using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Sello.Api.Contracts;

namespace Sello.Tests.Integration.Services
{
    public class ProductService : SelloService
    {
        public const string BaseUrl = "api/v1/product";
        private readonly SelloService _selloService = new SelloService();

        public async Task<List<ProductInformationContract>> GetAllAsync()
        {
            var response = await _selloService.GetResponseAsync(BaseUrl);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var rawContent = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductInformationContract>>(rawContent);
            return products;
        }
    }
}