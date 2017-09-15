using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Sello.Api.Owin.ExceptionHandling
{
    public class OwinExceptionHandler : ExceptionHandler
    {
        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, "The request could not be completed successfully, please try again.");
            context.Result = new ResponseMessageResult(response);

            return Task.CompletedTask;
        }
    }
}