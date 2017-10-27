using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Sello.Data.Managers;
using Sello.Domain.Model;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class ProductsController : ApiController
    {
        private readonly ProductsManager _productsManager = new ProductsManager();

        /// <summary>
        ///     Gets a list of all products
        /// </summary>
        [HttpGet]
        [Route("products")]
        [SwaggerResponse(HttpStatusCode.OK, "A list of all products", typeof(List<Product>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Get()
        {
            var products = await _productsManager.GetAsync();

            return Ok(products);
        }
    }
}