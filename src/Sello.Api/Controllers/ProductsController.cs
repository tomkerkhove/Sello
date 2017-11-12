using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Sello.Api.Contracts;
using Sello.Data.Managers;
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
        [SwaggerResponse(HttpStatusCode.OK, "A list of all products", typeof(List<ProductContract>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Get()
        {
            var storedProducts = await _productsManager.GetAsync();

            var products = storedProducts.Select(Mapper.Map<ProductContract>);

            return Ok(products);
        }
    }
}