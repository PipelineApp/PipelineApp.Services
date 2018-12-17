using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace PipelineApp.WebApi
{
    using Infrastructure.Providers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NLog.Config;
    using NLog.Web;

    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class WebApi : StatelessService
    {
        public WebApi(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("aspnet-user-id", typeof(AspNetUserIdLayoutRenderer));

            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton(serviceContext))
                                    .ConfigureAppConfiguration((builderContext, config) =>
                                    {
                                        var env = builderContext.HostingEnvironment;
                                        config.Sources.Clear();
                                        config
                                            .AddJsonFile("appsettings.json", false, true)
                                            .AddJsonFile("appsettings.secure.json", true, true)
                                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                                            .AddEnvironmentVariables();
                                    })
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .ConfigureLogging(logging =>
                                    {
                                        logging.ClearProviders();
                                        logging.SetMinimumLevel(LogLevel.Information);
                                    })
                                    .UseNLog()
                                    .Build();
                    }))
            };
        }
    }
}
