using System.Reflection;
using Banking.Account.Command.Application.Features.BankAccount.Commands.OpenAccount;
using Banking.Account.Command.Application.Models;
using EventBus.Base;
using EventBus.Kafka;
using EventBus.RabbitMQ;
using EventBus.RabbitMQ.Abstraction;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace Banking.Account.Command.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

        services.AddMediatR(Assembly.GetExecutingAssembly());
        var brokerType = configuration.GetSection("BrokerType").Value;
        if (brokerType == "Kafka")
        {
            ConfigureKafka(services, configuration);
            return services;
        }

        ConfigureRabbitMQ(services,configuration);

        return services;
    }

    private static void ConfigureRabbitMQ(IServiceCollection services,IConfiguration configuration)
    {
        var hostName = configuration.GetValue<string>("RabbitMQ:HostName");
        var subscriberClientAppName = configuration.GetValue<string>("RabbitMQ:SubscriberClientAppName");
        var userName = configuration.GetValue<string>("RabbitMQ:UserName");
        var password = configuration.GetValue<string>("RabbitMQ:Password");
        services.AddSingleton<IBaseEventBus>(sp =>
        {
            EventBusConfig config = new()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = subscriberClientAppName,
                EventBusType = EventBusType.RabbitMQ,
                DefaultTopicName = "BankingAccount",
                DefaultTopicType = "direct",
                Connection = new ConnectionFactory()
                {
                    HostName = hostName,
                    UserName = userName,
                    Password = password
                }
            };

            return EventBusFactory.Create(config, sp);
        });
    }

    private static void ConfigureKafka(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaEventProducerConfiguration>(configuration.GetSection("KafkaProducer"));
        services.Configure<KafkaEventProducerConfiguration>(options =>
        {
            options.SerializerSettings =
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        });
        services.AddTransient<IBaseEventBus, KafkaProducer>();
    }
}