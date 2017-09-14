using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1/products")]
    public class ProductsController : ApiController
    {
        /// <summary>
        ///     Gets a list of all products
        /// </summary>
        [SwaggerResponse(HttpStatusCode.OK, "A list of all products", typeof(List<string>))]
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