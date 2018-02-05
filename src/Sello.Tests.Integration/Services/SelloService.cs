using System;
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
        private const string ApiManagementAuthenticationKeyHeaderName = "ocp-apim-subscription-key";

        private const string ApiManagementAuthenticationKeySettingName = "Common.Api.Authenticationheader";

        private const string EnvironmentSettingName = "Common.Environment";
        private const string JsonContentType = "application/json";
        private const string UnleasheChaosMonkeysCustomHeader = "X-Inject-Chaos-Monkey";

        private readonly bool _chaosMonkeysUnleashed;
        private readonly HttpClient _httpClient = new HttpClient();

        public SelloService() : this(unleashChaosMonkeys: false)
        {
        }

        public SelloService(bool unleashChaosMonkeys)
        {
            _chaosMonkeysUnleashed = unleashChaosMonkeys;
        }

        public async Task<HttpResponseMessage> GetResponseAsync(string uriPath)
        {
            var url = GetUrl(uriPath);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (_chaosMonkeysUnleashed)
            {
                request.Headers.Add(UnleasheChaosMonkeysCustomHeader, Guid.NewGuid().ToString());
            }

            if (!IsLocalTest())
            {
                ConfigureApiManagementAuthentication(request);
            }

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

            if (_chaosMonkeysUnleashed)
            {
                request.Headers.Add(UnleasheChaosMonkeysCustomHeader, Guid.NewGuid().ToString());
            }

            if (!IsLocalTest())
            {
                ConfigureApiManagementAuthentication(request);
            }

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonContentType));

            var response = await _httpClient.SendAsync(request);
            return response;
        }

        private static void ConfigureApiManagementAuthentication(HttpRequestMessage request)
        {
            var apiManagementAuthenticationKey = ConfigurationManager.AppSettings[ApiManagementAuthenticationKeySettingName];
            request.Headers.Add(ApiManagementAuthenticationKeyHeaderName, apiManagementAuthenticationKey);
        }

        private string GetApiBaseUrl()
        {
            var apiBaseUrl = ConfigurationManager.AppSettings[ApiBaseUrlSettingName];
            return apiBaseUrl;
        }

        private string GetUrl(string uriPath)
        {
            var baseUrl = GetApiBaseUrl();
            var url = Url.Combine(baseUrl, uriPath);
            return url;
        }

        private bool IsLocalTest()
        {
            var environment = ConfigurationManager.AppSettings[EnvironmentSettingName];
            return environment.Equals("local", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}