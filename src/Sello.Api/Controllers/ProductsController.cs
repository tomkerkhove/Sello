using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class ProductsController : ApiController
    {
        /// <summary>
        ///     Gets a list of all products
        /// </summary>
        [HttpGet]
        [Route("products")]
        [SwaggerResponse(HttpStatusCode.OK, "A list of all products", typeof(List<string>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "The request could not be completed successfully, please try again.")]
        public IHttpActionResult Get()
        {
            var products = new List<string>
            {
                "Xbox One",
                "Surface Hub"
            };

            return Ok(products);
        }
    }
}