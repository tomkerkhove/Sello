using System.Web.Http;

namespace Sello.Api.Controllers
{
    public class RestApiController : ApiController
    {
        /// <summary>
        ///     Creates the URI for a specific resource
        /// </summary>
        /// <param name="resourceId">Identifier of a resource</param>
        protected string ComposeResourceLocation(string resourceId)
        {
            return $"{Request.RequestUri.OriginalString}/{resourceId}";
        }
    }
}