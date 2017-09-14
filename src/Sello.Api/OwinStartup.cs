using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin;
using Owin;
using Sello.Api;

[assembly: OwinStartup(typeof(OwinStartup))]
namespace Sello.Api
{
    public class OwinStartup
    {
        public virtual void Configuration(IAppBuilder app)
        {
            var httpConfiguration = GlobalConfiguration.Configuration;

            ConfigureRoutes(httpConfiguration);
            ConfigureExceptionHandling(httpConfiguration);
            ConfigureSwagger();

            httpConfiguration.EnsureInitialized();
        }

        private void ConfigureRoutes(HttpConfiguration httpConfiguration)
        {
            WebApiConfig.Register(httpConfiguration);
        }

        private void ConfigureExceptionHandling(HttpConfiguration httpConfiguration)
        {
           
        }

        private void ConfigureSwagger()
        {
            SwaggerConfig.Register();
        }
    }
}