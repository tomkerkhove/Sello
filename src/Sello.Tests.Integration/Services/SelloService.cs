using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;

namespace Sello.Tests.Integration.Services
{
    public class SelloService
    {
        private const string ApiBaseUrlSettingName = "Common.Api.BasePath";
        private const string JsonContentType = "application/json";
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<HttpResponseMessage> GetResponseAsync(string uriPath)
        {
            var url = GetUrl(uriPath);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> PostResponseAsync(string uriPath, object body)
        {
            var url = GetUrl(uriPath);

            var rawRequestContent = JsonConvert.SerializeObject(body);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(rawRequestContent, Encoding.UTF8, JsonContentType)
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));

            var response = await _httpClient.SendAsync(request);
            return response;
        }

        private string GetUrl(string uriPath)
        {
            var baseUrl = GetApiBaseUrl();
            var url = Url.Combine(baseUrl, uriPath);
            return url;
        }

        private string GetApiBaseUrl()
        {
            var apiBaseUrl = ConfigurationManager.AppSettings[ApiBaseUrlSettingName];
            return apiBaseUrl;
        }
    }
}