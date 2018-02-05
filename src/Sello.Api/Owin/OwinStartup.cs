using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Sello.Api.Contracts.Mapping;
using Sello.Api.Owin;
using Sello.Api.Owin.ExceptionHandling;
using Sello.Api.Swagger;
using Sello.Common.Configuration;
using Sello.Common.Configuration.Interfaces;
using Sello.Common.DependencyInjection;
using Sello.Common.Security;
using Sello.Common.Security.Interfaces;
using Sello.Common.Telemetry;
using Sello.Common.Telemetry.Interfaces;

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
            ConfigureDependencyContainer(app, httpConfiguration);

            httpConfiguration.EnsureInitialized();
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            InitializeKernel(kernel);

            return kernel;
        }

        private static void InitializeKernel(StandardKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<IConfigurationProvider>().To<LocalConfigurationProvider>();

            var configurationProvider = kernel.Get<IConfigurationProvider>();
            if (configurationProvider.IsCurrentEnvironment(Environment.Local))
            {
                kernel.Bind<ISecretProvider>().To<LocalSecretProvider>();
            }
            else
            {
                kernel.Bind<ISecretProvider>().To<KeyVaultSecretProvider>();
            }

            kernel.Bind<ITelemetry>().To<ApplicationInsightsTelemetry>();
        }

        private void ConfigureDependencyContainer(IAppBuilder app, HttpConfiguration httpConfiguration)
        {
            InitializeKernel(PlatformKernel.Instance);
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(httpConfiguration);
        }

        private void ConfigureExceptionHandling(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.Services.Replace(typeof(IExceptionHandler), new OwinExceptionHandler());
        }

        private void ConfigureMapping()
        {
            ContractMapper.Configure();
        }

        private void ConfigureRoutes(HttpConfiguration httpConfiguration)
        {
            WebApiConfig.Register(httpConfiguration);
        }

        private void ConfigureSwagger()
        {
            SwaggerConfig.Register();
        }
    }
}