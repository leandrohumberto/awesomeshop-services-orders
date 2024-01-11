using AwesomeShop.Services.Orders.Application.Dtos.IntegrationDtos;
using AwesomeShop.Services.Orders.Application.Extensions;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;
using MediatR;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Application.Commands.Handlers
{
    public class AddOrderHandler : IRequestHandler<AddOrder, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBusClient _messageBus;
        private readonly IServiceDiscoveryService _serviceDiscovery;
        private readonly IHttpClientFactory _httpClientFactory;

        public AddOrderHandler(IOrderRepository orderRepository, IMessageBusClient messageBus, IServiceDiscoveryService serviceDiscovery, IHttpClientFactory httpClientFactory)
        {
            _orderRepository = orderRepository;
            _messageBus = messageBus;
            _serviceDiscovery = serviceDiscovery;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Guid> Handle(AddOrder request, CancellationToken cancellationToken)
        {
            var order = request.ToEntity();

            var customerUrl = await _serviceDiscovery
                .GetServiceUri("CustomerServices", $"api/customers/{order.Customer.Id}");

            using var httpClient = _httpClientFactory.CreateClient("AddOrder");

            var result = await httpClient.GetAsync(customerUrl, cancellationToken);
            var stringResult = await result.Content.ReadAsStringAsync(cancellationToken);

            var customerDto = JsonSerializer.Deserialize<GetCustomerByIdDto>(stringResult);

            Console.WriteLine(customerDto.FullName);

            await _orderRepository.AddAsync(order, cancellationToken);

            foreach (var @event in order.Events)
            {
                var routingkey = @event.GetType().Name.ToDashCase();

                _messageBus.Publish(@event, routingkey, "order-service");
            }

            return order.Id;
        }
    }
}
