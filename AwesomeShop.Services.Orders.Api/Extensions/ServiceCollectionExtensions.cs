using AwesomeShop.Services.Orders.Application.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeShop.Services.Orders.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services)
            => services.AddMediatR(typeof(AddOrder));
    }
}
