using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Sello.Api.Contracts;
using Sello.Data.Repositories;
using Sello.Datastore.SQL.Model;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class ProductsController : RestApiController
    {
        private readonly ProductsRepository _productsRepository = new ProductsRepository();

        /// <summary>
        ///     Gets a list of all products
        /// </summary>
        [HttpGet]
        [Route("product")]
        [SwaggerResponse(HttpStatusCode.OK, "A list of all products", typeof(List<ProductContract>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Get()
        {
            var storedProducts = await _productsRepository.GetAsync();

            var products = storedProducts.Select(Mapper.Map<ProductContract>);

            return Ok(products);
        }

        /// <summary>
        ///     Gets the details for a specific product in the catalog
        /// </summary>
        [HttpGet]
        [Route("product/{productId}")]
        [SwaggerResponse(HttpStatusCode.OK, "Details about a product", typeof(ProductContract))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Get(int productId)
        {
            var storedProduct = await _productsRepository.GetAsync(productId);
            if (storedProduct == null)
            {
                return NotFound();
            }

            var product = Mapper.Map<ProductContract>(storedProduct);

            return Ok(product);
        }

        /// <summary>
        ///     Add a new product to the catalog a list of all products
        /// </summary>
        [HttpPost]
        [Route("product")]
        [SwaggerResponse(HttpStatusCode.Created, "A list of all products", typeof(ProductContract))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "No valid product was specified.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Post(ProductContract newProduct)
        {
            var product = Mapper.Map<Product>(newProduct);
            var storedProduct = await _productsRepository.AddAsync(product);

            var resourceLocation = ComposeResourceLocation(storedProduct.Id.ToString());
            return Created(resourceLocation, newProduct);
        }
    }
}