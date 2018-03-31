using System.Net;
using System.Web.Http;
using Sello.Common.Configuration.Interfaces;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class HealthController : RestApiController
    {
        public HealthController(IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
        }

        /// <summary>
        ///     Get Health
        /// </summary>
        /// <remarks>Gets the current health status of the API</remarks>
        [HttpGet]
        [Route("health")]
        [SwaggerOperation("get-health")]
        [SwaggerResponse(HttpStatusCode.OK, "API is up & running")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "API is not available")]
#if MANAGEMENT_API || PUBLIC_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public IHttpActionResult Get()
        {
            if (IsChaosMonkeyUnleashed())
            {
                return InternalServerError();
            }

            // Return OK for now, if the API is unhealthy, it will return HTTP 500 anyway
            return Ok();
        }
    }
}