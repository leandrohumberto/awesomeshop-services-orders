using AwesomeShop.Services.Orders.Application.Events;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Application.Subscribers
{
    public class PaymentAcceptedSubscriber : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private const string QUEUE = "order-service/payment-accepted";
        private const string EXCHANGE = "order-service";
        private const string ROUTING_KEY = "payment-accepted";

        public PaymentAcceptedSubscriber(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
            };

            _connection = connectionFactory.CreateConnection("order-service-payment-accepted-subscriber");
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(EXCHANGE, "topic", durable: true, autoDelete: false, arguments: null);
            _channel.QueueDeclare(QUEUE,
                durable: true, autoDelete: false, exclusive: false, arguments: null);
            _channel.QueueBind(QUEUE, exchange: "payment-service", ROUTING_KEY, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;
            _channel.BasicConsume(QUEUE, false, consumer);
            return Task.CompletedTask;
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var byteArray = e.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(byteArray);
            var message = JsonSerializer.Deserialize<PaymentAccepted>(contentString);

            Console.WriteLine($"Message PaymentAccepted received with Id {message.Id}.");

            await UpdateOrder(message);

            _channel.BasicAck(e.DeliveryTag, false);
        }

        private async Task<bool> UpdateOrder(PaymentAccepted paymentAccepted)
        {
            using var scope = _serviceProvider.CreateScope();
            var orderRepository = scope.ServiceProvider.GetService<IOrderRepository>();
            var order = await orderRepository.GetByIdAsync(paymentAccepted.Id);

            if (order != null)
            {
                order.SetAsCompleted();
                await orderRepository.UpdateAsync(order);
                return true;
            }

            return false;
        }
    }
}
