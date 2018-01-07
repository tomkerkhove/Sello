using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin;
using Ninject;
using Owin;
using Sello.Api.Contracts.Mapping;
using Sello.Api.Owin;
using Sello.Api.Owin.ExceptionHandling;
using Sello.Common.Configuration;
using Sello.Common.Configuration.Interfaces;
using Sello.Common.DependencyInjection;
using Sello.Common.Security;
using Sello.Common.Security.Interfaces;

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
            ConfigureDependencyContainer();

            httpConfiguration.EnsureInitialized();
        }

        private void ConfigureDependencyContainer()
        {
            PlatformKernel.Instance.Bind<IConfigurationProvider>().To<LocalConfigurationProvider>();

            var configurationProvider=PlatformKernel.Instance.Get<IConfigurationProvider>();
            if (configurationProvider.IsCurrentEnvironment(Environment.Local))
            {
                PlatformKernel.Instance.Bind<ISecretProvider>().To<LocalSecretProvider>();
            }
            else
            {
                PlatformKernel.Instance.Bind<ISecretProvider>().To<KeyVaultSecretProvider>();
            }
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