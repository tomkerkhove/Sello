using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json;
using Sello.Api.Contracts;
using Sello.Api.Validators;
using Sello.Data.Repositories;
using Sello.Datastore.SQL.Model;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class OrdersController : RestApiController
    {
        private readonly CustomerValidator _customerValidator = new CustomerValidator();
        private readonly OrdersRepository _ordersRepository = new OrdersRepository();
        private readonly ProductsRepository _productsRepository = new ProductsRepository();
        private readonly ProductValidator _productValidator = new ProductValidator();

        /// <summary>
        ///     Creates a new order
        /// </summary>
        [HttpPost]
        [Route("order")]
        [SwaggerResponse(HttpStatusCode.Created, "Order was created")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Order failed to validate", typeof(List<string>))]
        [SwaggerResponse(HttpStatusCode.NotFound, "Product that was mentioned in the order was not found")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Post([FromBody] OrderContract order)
        {
            var validationResult = await ValidateOrderAsync(order);
            if (validationResult != null)
            {
                return validationResult;
            }

            var orderConfirmation = await StoreOrderAsync(order);

            var resourceUri = ComposeResourceLocation(orderConfirmation.ConfirmationId);

            return Created(resourceUri, orderConfirmation);
        }

        private async Task<IHttpActionResult> ValidateOrderAsync(OrderContract order)
        {
            if (order == null)
            {
                return BadRequest("No order was specified");
            }

            var persistedProduct = await _productsRepository.GetAsync(order.Product?.Id);
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

        private async Task<OrderConfirmationContract> StoreOrderAsync(OrderContract order)
        {
            var confirmationId = Guid.NewGuid().ToString();

            var dbOrder = Mapper.Map<Order>(order);
            dbOrder.ConfirmationId = confirmationId;

            await _ordersRepository.AddAsync(dbOrder);

            var orderConfirmation = new OrderConfirmationContract
            {
                ConfirmationId = confirmationId,
                Order = order
            };

            return orderConfirmation;
        }
    }
}