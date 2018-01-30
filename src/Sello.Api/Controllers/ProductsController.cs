using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Sello.Api.Contracts;
using Sello.Common.Telemetry.Interfaces;
using Sello.Data.Repositories;
using Sello.Datastore.SQL.Model;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class ProductsController : RestApiController
    {
        private const string ProductAddedEvent = "Product Added";
        private readonly ITelemetry _telemetry;
        private ProductsRepository _productsRepository;

        public ProductsController(ITelemetry telemetry)
        {
            this._telemetry = telemetry;
        }

        /// <summary>
        ///     Gets a list of all products
        /// </summary>
        [HttpGet]
        [Route("product")]
        [SwaggerResponse(HttpStatusCode.OK, "A list of all products", typeof(List<ProductInformationContract>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
#if MANAGEMENT_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public async Task<IHttpActionResult> Get()
        {
            var productsRepository = await GetOrCreateProductsRepositoryAsync();
            var storedProducts = await productsRepository.GetAsync();

            var products = storedProducts.Select(Mapper.Map<ProductInformationContract>);

            return Ok(products);
        }

        /// <summary>
        ///     Gets the details for a specific product in the catalog
        /// </summary>
        [HttpGet]
        [Route("product/{productId}")]
        [SwaggerResponse(HttpStatusCode.OK, "Details about a product", typeof(ProductInformationContract))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
#if MANAGEMENT_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public async Task<IHttpActionResult> Get(string productId)
        {
            var productsRepository = await GetOrCreateProductsRepositoryAsync();
            var storedProduct = await productsRepository.GetAsync(productId);
            if (storedProduct == null)
            {
                return NotFound();
            }

            var product = Mapper.Map<ProductInformationContract>(storedProduct);

            return Ok(product);
        }

        /// <summary>
        ///     Add a new product to the catalog a list of all products
        /// </summary>
        [HttpPost]
        [Route("product")]
        [SwaggerResponse(HttpStatusCode.Created, "Information about the added product", typeof(NewProductContract))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "No valid product was specified.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
#if PUBLIC_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public async Task<IHttpActionResult> Post(NewProductContract newProduct)
        {
            var product = Mapper.Map<Product>(newProduct);
            product.ExternalId = Guid.NewGuid().ToString();

            var productsRepository = await GetOrCreateProductsRepositoryAsync();
            var storedProduct = await productsRepository.AddAsync(product);
            var productInformation = Mapper.Map<ProductInformationContract>(storedProduct);

            TrackNewProductAddedEvent(productInformation);

            var resourceLocation = ComposeResourceLocation(productInformation.Id);
            return Created(resourceLocation, productInformation);
        }

        private async Task<ProductsRepository> GetOrCreateProductsRepositoryAsync()
        {
            if (_productsRepository == null)
            {
                _productsRepository = await ProductsRepository.CreateAsync();
            }

            return _productsRepository;
        }

        private void TrackNewProductAddedEvent(ProductInformationContract productInformation)
        {
            var eventContext = new Dictionary<string, string>
            {
                {"Name", productInformation.Name},
                {"Id", productInformation.Id},
                {"Description", productInformation.Description},
                {"Price", productInformation.Price.ToString("C")}
            };

            _telemetry.TrackEvent(ProductAddedEvent, eventContext);
        }
    }
}