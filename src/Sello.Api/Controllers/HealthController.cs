using System.Net;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class HealthController : RestApiController
    {
        /// <summary>
        /// Gets the current health status of the API
        /// </summary>
        [HttpGet]
        [Route("health")]
        [SwaggerOperation("Get Health")]
        [SwaggerResponse(HttpStatusCode.OK, "API is up & running")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "API is not available")]
#if MANAGEMENT_API || PUBLIC_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public IHttpActionResult Get()
        {
            // Return OK for now, if the API is unhealthy, it will return HTTP 500 anyway
            return Ok();
        }
    }
}