using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI;
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

        /// <summary>
        ///     Gets the details for a specific product in the catalog
        /// </summary>
        [HttpGet]
        [Route("products/{productId}")]
        [SwaggerResponse(HttpStatusCode.OK, "Details about a product", typeof(ProductContract))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Get(int productId)
        {
            var storedProduct = await _productsManager.GetAsync(productId);

            var product = Mapper.Map<ProductContract>(storedProduct);

            return Ok(product);
        }

        /// <summary>
        ///     Add a new product to the catalog a list of all products
        /// </summary>
        [HttpPost]
        [Route("products")]
        [SwaggerResponse(HttpStatusCode.Created, "A list of all products", typeof(ProductContract))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "No valid product was specified.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Post(ProductContract newProduct)
        {
            await Task.CompletedTask;

            var resourceLocation = ComposeResourceLocation(1);

            return Created(resourceLocation, newProduct);
        }

        private string ComposeResourceLocation(int resourceId)
        {
            return $"{Request.RequestUri.OriginalString}/{resourceId}";
        }
    }
}