using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;

namespace Sello.Tests.Integration
{
    public class SelloService
    {
        private const string ApiBaseUrlSettingName = "Common.Api.BasePath";
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<HttpResponseMessage> GetResponseAsync(string uriPath)
        {
            var baseUrl = GetApiBaseUrl();
            var url = Url.Combine(baseUrl, uriPath);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        private string GetApiBaseUrl()
        {
            var apiBaseUrl = ConfigurationManager.AppSettings[ApiBaseUrlSettingName];
            return apiBaseUrl;
        }
    }
}