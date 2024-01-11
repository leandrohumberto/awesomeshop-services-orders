using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AwesomeShop.Services.Orders.Infrastructure.MessageBus
{
    public class RabbitMQClient : IMessageBusClient
    {
        public IConnection Connection { get; }

        public RabbitMQClient(ProducerConnection producerConnection)
        {
            Connection = producerConnection.Connection;
        }

        public void Publish(object message, string routingKey, string exchange)
        {
            var channel = Connection.CreateModel();
            var serializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var payload = JsonSerializer.Serialize(message, serializerOptions);
            var body = Encoding.UTF8.GetBytes(payload);

            channel.ExchangeDeclare(exchange, "topic", durable: true, autoDelete: false);
            channel.BasicPublish(exchange, routingKey, null, body);
        }
    }
}
