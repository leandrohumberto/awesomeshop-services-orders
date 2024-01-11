using AwesomeShop.Services.Orders.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using AwesomeShop.Services.Orders.Core.Repositories;
using AwesomeShop.Services.Orders.Infrastructure.Persistence.Repositories;
using RabbitMQ.Client;
using AwesomeShop.Services.Orders.Infrastructure.MessageBus;
using System;
using AwesomeShop.Services.Orders.Infrastructure.Persistence.SerializationProviders;
using Consul;
using Microsoft.AspNetCore.Builder;
using AwesomeShop.Services.Orders.Infrastructure.ServiceDiscovery;

namespace AwesomeShop.Services.Orders.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            // Criar um serviço singleton com as opções de conexão do MongoDb (objeto MongoDbOptions)
            // Opções: Database e ConnectionString
            // Os valores serão sempre os mesmos, vindos de configurações pré-definidas
            services.AddSingleton<MongoDbOptions>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var options = new MongoDbOptions();
                configuration.GetSection("Mongo").Bind(options);
                return options;
            });

            // Criar um IMongoClient como serviço singleton, utilizando as opções previamente registradas
            // (ConnectionString)
            services.AddSingleton<IMongoClient>(sp =>
            {
                var options = sp.GetService<MongoDbOptions>();
                return new MongoClient(options.ConnectionString);
            });

            // Definir a serialização do GUID e registrar instância do IMongoDatabase
            services.AddTransient<IMongoDatabase>(sp =>
            {
                //BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

                BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;

                //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                BsonSerializer.RegisterSerializationProvider(new CustomGuidSerializationProvider());

                var options = sp.GetService<MongoDbOptions>();
                var client = sp.GetService<IMongoClient>();
                return client.GetDatabase(options.Database);
            });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
            };

            var connection = connectionFactory.CreateConnection("order-service-producer");
            services.AddSingleton(new ProducerConnection(connection));
            services.AddSingleton<IMessageBusClient, RabbitMQClient>();

            return services;
        }

        /// <summary>
        /// Add Consul client and its configurations. Extension method.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig => 
            {
                var address = configuration.GetValue<string>("Consul:Host");
                consulConfig.Address = new Uri(address);
            }));

            services.AddTransient<IServiceDiscoveryService, ConsulService>();

            return services;
        }
    }
}
