using System;
using System.Collections.Generic;
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
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public async Task<IHttpActionResult> Post([FromBody] OrderContract order)
        {
            var confirmationId = Guid.NewGuid().ToString();

            await StoreOrderAsync(order, confirmationId);

            var orderConfirmation = new OrderConfirmationContract
            {
                ConfirmationId = confirmationId,
                Customer = order.Customer,
                Item = order.Item
            };

            var resourceUri = ComposeResourceLocation(confirmationId);

            return Created(resourceUri, orderConfirmation);
        }

        private async Task StoreOrderAsync(OrderContract order, string confirmationId)
        {
            var dbOrder = Mapper.Map<Order>(order);
            dbOrder.ConfirmationId = confirmationId;


            await _ordersRepository.AddAsync(dbOrder);
        }
    }
}