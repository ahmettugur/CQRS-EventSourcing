using Banking.Account.Consumer.Application.Contracts.Persistence;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.AccountClosed;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.AccountOpened;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.FundsDeposited;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.FundsWithdraw;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.ReplayAccount;
using Banking.Cqrs.Core.Events;
using EventBus.Kafka;
using MediatR;
using System.Reflection;
using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.OpenAccount;
using EventBus.RabbitMQ;
using EventBus.RabbitMQ.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;


namespace Banking.Account.Consumer.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddKafka(this IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<KafkaEventConsumerConfiguration>(configuration.GetSection("KafkaConsumer"));
        services.Configure<KafkaEventConsumerConfiguration>(options =>
        {
            options.RegisterConsumer<AccountOpenedEvent, AccountOpenedEventHandler>();
            options.RegisterConsumer<AccountClosedEvent, AccountClosedEventHandler>();
            options.RegisterConsumer<FundsDepositedEvent, FundsDepositedEventHandler>();
            options.RegisterConsumer<FundsWithdrawnEvent, FundsWithdrawnEventHandler>();
            options.RegisterConsumer<ReplayAccountEvent, ReplayAccountEventHandler>();
        });
        services.AddSingleton<IHostedService, KafkaConsumer>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddTransient<AccountOpenedEventHandler>();
        services.AddTransient<AccountClosedEventHandler>();
        services.AddTransient<FundsDepositedEventHandler>();
        services.AddTransient<FundsWithdrawnEventHandler>();
        services.AddTransient<ReplayAccountEventHandler>();

        return services;
    }
    
    public static IServiceCollection AddRabbitMq(this IServiceCollection services,IConfiguration configuration)
    {

        var hostName = configuration.GetValue<string>("RabbitMQ:HostName");
        var subscriberClientAppName = configuration.GetValue<string>("RabbitMQ:SubscriberClientAppName");
        var userName = configuration.GetValue<string>("RabbitMQ:UserName");
        var password = configuration.GetValue<string>("RabbitMQ:Password");
        services.AddSingleton<IEventBus>(sp =>
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

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddTransient<AccountOpenedEventHandler>();
        services.AddTransient<AccountClosedEventHandler>();
        services.AddTransient<FundsDepositedEventHandler>();
        services.AddTransient<FundsWithdrawnEventHandler>();
        services.AddTransient<ReplayAccountEventHandler>();
        
        return services;
    }
    
    public static void SubscribeRabbitMqEvent(this IHost app)
    {
        var eventBus = app.Services.GetRequiredService<IEventBus>();
        eventBus.Subscribe<AccountOpenedEvent, AccountOpenedEventHandler>();
        eventBus.Subscribe<AccountClosedEvent,AccountClosedEventHandler>();
        eventBus.Subscribe<FundsDepositedEvent,FundsDepositedEventHandler>();
        eventBus.Subscribe<FundsWithdrawnEvent,FundsWithdrawnEventHandler>();
        eventBus.Subscribe<ReplayAccountEvent, ReplayAccountEventHandler>();
    }
}