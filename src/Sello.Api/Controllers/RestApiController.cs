using System.Linq;
using System.Web.Http;
using Sello.Common.Configuration.Interfaces;

namespace Sello.Api.Controllers
{
    public class RestApiController : ApiController
    {
        public RestApiController(IConfigurationProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
        }

        protected IConfigurationProvider ConfigurationProvider { get; }

        /// <summary>
        ///     Creates the URI for a specific resource
        /// </summary>
        /// <param name="resourceId">Identifier of a resource</param>
        protected string ComposeResourceLocation(string resourceId)
        {
            return $"{Request.RequestUri.OriginalString}/{resourceId}";
        }

        protected bool IsChaosMonkeyUnleashed()
        {
            if (Request != null && Request.Headers.TryGetValues("X-Inject-Chaos-Monkey", out var headerValues))
            {
                var isChaosMonkeyInjected = headerValues.FirstOrDefault();
                return !string.IsNullOrWhiteSpace(isChaosMonkeyInjected);
            }

            var rawSetting = ConfigurationProvider.GetSetting("Demo.UnleashChaosMonkey", isMandatory: false);
            if (bool.TryParse(rawSetting, out var isChaosMonkeyUnleashed))
            {
                return isChaosMonkeyUnleashed;
            }

            return false;
        }
    }
}