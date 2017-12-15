using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json;
using Sello.Api.Contracts;
using Sello.Data.Repositories;
using Sello.Datastore.SQL.Model;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class OrdersController : RestApiController
    {
        private readonly OrdersRepository _ordersRepository = new OrdersRepository();
        private readonly ProductsRepository _productsRepository = new ProductsRepository();

        /// <summary>
        ///     Creates a new order
        /// </summary>
        [HttpPost]
        [Route("order")]
        [SwaggerResponse(HttpStatusCode.Created, "Order was created")]
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

            var persistedProduct = await _productsRepository.GetAsync(order.Product.Id);
            if (persistedProduct == null)
            {
                return NotFound();
            }

            var validationMessages = new List<string>();

            var productValidationMessages = ValidateProduct(order.Product, persistedProduct);
            validationMessages.AddRange(productValidationMessages);

            var customerValidationMessages = ValidateCustomer(order.Customer);
            validationMessages.AddRange(customerValidationMessages);

            if (validationMessages.Any())
            {
                var rawResponse = JsonConvert.SerializeObject(validationMessages);
                return BadRequest(rawResponse);
            }

            return null;
        }

        private List<string> ValidateProduct(ProductInformationContract product, Product persistedProduct)
        {
            var validationMessages = new List<string>();

            if (product == null)
            {
                validationMessages.Add("No product was specified");
            }
            else
            {
                if (product.Name != persistedProduct.Name)
                {
                    validationMessages.Add("Name of product is not correct");
                }
                if (product.Description != persistedProduct.Description)
                {
                    validationMessages.Add("Description of product is not correct");
                }
                if (product.Price != persistedProduct.Price)
                {
                    validationMessages.Add("Price of product is not correct");
                }
            }

            return validationMessages;
        }

        private List<string> ValidateCustomer(CustomerContract customer)
        {
            var validationMessages = new List<string>();

            if (customer == null)
            {
                validationMessages.Add("No customer was specified");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(customer.EmailAddress))
                {
                    validationMessages.Add("Email address of the customer is not specified");
                }
                if (string.IsNullOrWhiteSpace(customer.FirstName))
                {
                    validationMessages.Add("First name of the customer is not specified");
                }
                if (string.IsNullOrWhiteSpace(customer.LastName))
                {
                    validationMessages.Add("Last name of the customer is not specified");
                }
            }

            return validationMessages;
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