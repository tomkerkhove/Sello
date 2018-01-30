using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Sello.Api.Contracts;
using Sello.Api.Validators;
using Sello.Common.Telemetry.Interfaces;
using Sello.Data.Repositories;
using Sello.Datastore.SQL.Model;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class OrdersController : RestApiController
    {
        private const string OrderCreatedEvent = "Order Created";
        private readonly CustomerValidator _customerValidator = new CustomerValidator();
        private readonly ProductValidator _productValidator = new ProductValidator();
        private readonly ITelemetry _telemetry;
        private OrdersRepository _ordersRepository;
        private ProductsRepository _productsRepository;

        public OrdersController(ITelemetry telemetry)
        {
            this._telemetry = telemetry;
        }

        /// <summary>
        ///     Creates a new order
        /// </summary>
        [HttpPost]
        [Route("order")]
        [SwaggerOperation("Create Order")]
        [SwaggerResponse(HttpStatusCode.Created, "Order was created")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Order failed to validate", typeof(List<string>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product that was mentioned in the order was not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
#if MANAGEMENT_API
        [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)]
#endif
        public async Task<IHttpActionResult> Post([FromBody] OrderContract order)
        {
            var validationResult = await ValidateOrderAsync(order);
            if (validationResult != null)
            {
                return validationResult;
            }

            var orderConfirmation = await StoreOrderAsync(order);

            var resourceUri = ComposeResourceLocation(orderConfirmation.ConfirmationId);

            TrackNewOrderCreatedEvent(orderConfirmation);

            return Created(resourceUri, orderConfirmation);
        }

        private async Task<OrdersRepository> GetOrCreateOrdersRepositoryAsync()
        {
            if (_ordersRepository == null)
            {
                _ordersRepository = await OrdersRepository.CreateAsync();
            }

            return _ordersRepository;
        }

        private async Task<ProductsRepository> GetOrCreateProductsRepositoryAsync()
        {
            if (_productsRepository == null)
            {
                _productsRepository = await ProductsRepository.CreateAsync();
            }

            return _productsRepository;
        }

        private async Task<OrderConfirmationContract> StoreOrderAsync(OrderContract order)
        {
            var confirmationId = Guid.NewGuid().ToString();

            var dbOrder = Mapper.Map<Order>(order);
            dbOrder.ConfirmationId = confirmationId;

            var ordersRepository = await GetOrCreateOrdersRepositoryAsync();
            await ordersRepository.AddAsync(dbOrder);

            var orderConfirmation = new OrderConfirmationContract
            {
                ConfirmationId = confirmationId,
                Order = order
            };

            return orderConfirmation;
        }

        private void TrackNewOrderCreatedEvent(OrderConfirmationContract orderConfirmation)
        {
            var eventContext = new Dictionary<string, string>
            {
                {"ConfirmationId", orderConfirmation.ConfirmationId},
                {"EmailAddress", orderConfirmation.Order.Customer.EmailAddress},
                {"Product", orderConfirmation.Order.Product.Name},
                {"ProductId", orderConfirmation.Order.Product.Id}
            };

            _telemetry.TrackEvent(OrderCreatedEvent, eventContext);
        }

        private async Task<IHttpActionResult> ValidateOrderAsync(OrderContract order)
        {
            if (order == null)
            {
                return BadRequest("No order was specified");
            }

            var productsRepository = await GetOrCreateProductsRepositoryAsync();
            var persistedProduct = await productsRepository.GetAsync(order.Product?.Id);
            if (persistedProduct == null)
            {
                return NotFound();
            }

            var validationMessages = new List<string>();

            var productValidationMessages = _productValidator.Run(order.Product, persistedProduct);
            validationMessages.AddRange(productValidationMessages);

            var customerValidationMessages = _customerValidator.Run(order.Customer);
            validationMessages.AddRange(customerValidationMessages);

            if (validationMessages.Any())
            {
                return Content(HttpStatusCode.BadRequest, validationMessages);
            }

            return null;
        }
    }
}