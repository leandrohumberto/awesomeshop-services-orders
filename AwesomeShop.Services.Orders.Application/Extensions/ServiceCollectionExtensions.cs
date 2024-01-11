using AwesomeShop.Services.Orders.Application.Subscribers;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeShop.Services.Orders.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSubscribers(this IServiceCollection services)
            => services.AddHostedService<PaymentAcceptedSubscriber>();
    }
}
