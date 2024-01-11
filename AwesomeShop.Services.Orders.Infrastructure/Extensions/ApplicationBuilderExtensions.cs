using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AwesomeShop.Services.Orders.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Implements application self-registration to Consul service discovery.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            var registration = new AgentServiceRegistration()
            {
                ID = $"order-service-{Guid.NewGuid()}",
                Name = "OrderServices",
                Address = "localhost",
                Port = 5003
            };

            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            Console.WriteLine($"Service Registered in Consul - ID: {registration.ID}");

            lifetime.ApplicationStopping.Register(() => {
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
                Console.WriteLine("Service Deregistered in Consul");
            });

            return app;
        }
    }
}
