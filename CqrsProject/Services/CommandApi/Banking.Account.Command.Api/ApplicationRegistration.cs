using Banking.Account.Command.Application.Features.BankAccount.Commands.OpenAccount;
using Banking.Account.Command.Application.Models;
using Banking.Account.Command.Infrastructure;
using EventBus.Base;
using EventBus.Kafka;
using EventBus.RabbitMQ;
using EventBus.RabbitMQ.Abstraction;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace Banking.Account.Command.Api;

public static class ApplicationRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

        services.AddMediatR(typeof(OpenAccountCommand).Assembly);
        
        services.AddInfrastructureServices(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        var brokerType = configuration.GetSection("BrokerType").Value;
        if (brokerType == "Kafka")
        {
            ConfigureKafka(services, configuration);
            return services;
        }

        ConfigureRabbitMQ(services);

        return services;
    }

    private static void ConfigureRabbitMQ(IServiceCollection services)
    {
        services.AddSingleton<IBaseEventBus>(sp =>
        {
            EventBusConfig config = new()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "BankingAccount",
                EventBusType = EventBusType.RabbitMQ,
                Connection = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest"
                }
            };

            return EventBusFactory.Create(config, sp);
        });
    }

    private static void ConfigureKafka(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaEventProducerConfiguration>(configuration.GetSection("KafkaProducer"));
        services.PostConfigure<KafkaEventProducerConfiguration>(options =>
        {
            options.SerializerSettings =
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        });
        services.AddTransient<IBaseEventBus, KafkaProducer>();
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors("CorsPolicy");

        app.MapControllers();
        
        return app;
    }
}