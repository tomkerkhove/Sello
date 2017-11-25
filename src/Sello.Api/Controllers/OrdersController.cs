using System;
using System.Net;
using System.Web.Http;
using Sello.Api.Contracts;
using Swashbuckle.Swagger.Annotations;

namespace Sello.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class OrdersController : RestApiController
    {
        /// <summary>
        ///     Creates a new order
        /// </summary>
        [HttpPost]
        [Route("orders")]
        [SwaggerResponse(HttpStatusCode.Created, "Order was created")]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
            "The request could not be completed successfully, please try again.")]
        public IHttpActionResult Post([FromBody] OrderContract order)
        {
            var confirmationId = Guid.NewGuid().ToString();

            var orderConfirmation = new OrderConfirmationContract
            {
                ConfirmationId = confirmationId,
                Customer = order.Customer,
                Products = order.Products
            };

            var resourceUri = ComposeResourceLocation(confirmationId);

            return Created(resourceUri, orderConfirmation);
        }
    }
}