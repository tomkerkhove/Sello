using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Sello.Api.Contracts;
using Sello.Api.Exceptions;
using Sello.Common.Telemetry.Interfaces;
using Sello.Data.Repositories;
using Sello.Datastore.SQL.Model;
using Swashbuckle.Swagger.Annotations;
using IConfigurationProvider = Sello.Common.Configuration.Interfaces.IConfigurationProvider;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class ProductsController : RestApiController
    {
        private const string ProductAddedEvent = "Product Added";
        private readonly ITelemetry _telemetry;
        private ProductsRepository _productsRepository;

        public ProductsController(ITelemetry telemetry, IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
            _telemetry = telemetry;
        }

        /// <summary>
        ///     Get All Products
        /// </summary>
        /// <remarks>Gets a list of all products</remarks>
        [HttpGet]
        [Route("product")]
        [SwaggerOperation("get-all-products")]
        [SwaggerResponse(HttpStatusCode.OK, "A list of all products", typeof(List<ProductInformationContract>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
#if MANAGEMENT_API || OPERATIONS_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public async Task<IHttpActionResult> Get()
        {
            if (IsChaosMonkeyUnleashed())
            {
                throw new ChaosMonkeyException();
            }

            var productsRepository = await GetOrCreateProductsRepositoryAsync();
            var storedProducts = await productsRepository.GetAsync();

            var products = storedProducts.Select(Mapper.Map<ProductInformationContract>);

            return Ok(products);
        }

        /// <summary>
        ///     Get Product
        /// </summary>
        /// <remarks>Gets the details for a specific product in the catalog</remarks>
        [HttpGet]
        [Route("product/{productId}")]
        [SwaggerOperation("get-product")]
        [SwaggerResponse(HttpStatusCode.OK, "Details about a product", typeof(ProductInformationContract))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
#if MANAGEMENT_API || OPERATIONS_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public async Task<IHttpActionResult> Get(string productId)
        {
            if (IsChaosMonkeyUnleashed())
            {
                throw new ChaosMonkeyException();
            }

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
        ///     Add New Product
        /// </summary>
        /// <remarks>Add a new product to the catalog a list of all products</remarks>
        [HttpPost]
        [Route("product")]
        [SwaggerOperation("add-new-product")]
        [SwaggerResponse(HttpStatusCode.Created, "Information about the added product", typeof(NewProductContract))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "No valid product was specified.")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
#if PUBLIC_API || OPERATIONS_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public async Task<IHttpActionResult> Post(NewProductContract newProduct)
        {
            if (IsChaosMonkeyUnleashed())
            {
                throw new ChaosMonkeyException();
            }

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