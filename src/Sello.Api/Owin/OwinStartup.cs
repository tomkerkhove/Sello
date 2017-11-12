using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin;
using Owin;
using Sello.Api.Contracts.Mapping;
using Sello.Api.Owin;
using Sello.Api.Owin.ExceptionHandling;

[assembly: OwinStartup(typeof(OwinStartup))]

namespace Sello.Api.Owin
{
    public class OwinStartup
    {
        public virtual void Configuration(IAppBuilder app)
        {
            var httpConfiguration = GlobalConfiguration.Configuration;

            ConfigureRoutes(httpConfiguration);
            ConfigureExceptionHandling(httpConfiguration);
            ConfigureSwagger();
            ConfigureMapping();

            httpConfiguration.EnsureInitialized();
        }

        private void ConfigureMapping()
        {
            ContractMapper.Configure();
        }

        private void ConfigureRoutes(HttpConfiguration httpConfiguration)
        {
            WebApiConfig.Register(httpConfiguration);
        }

        private void ConfigureExceptionHandling(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Services.Replace(typeof(IExceptionHandler), new OwinExceptionHandler());
        }

        private void ConfigureSwagger()
        {
            SwaggerConfig.Register();
        }
    }
}