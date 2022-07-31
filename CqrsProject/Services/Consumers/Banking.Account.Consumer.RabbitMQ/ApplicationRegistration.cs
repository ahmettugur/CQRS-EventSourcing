using System.Reflection;
using Banking.Account.Consumer.Application;
using Banking.Account.Consumer.Application.Contracts.Persistence;
using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.AccountClosed;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.AccountOpened;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.FundsDeposited;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.FundsWithdraw;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.ReplayAccount;
using Banking.Account.Consumer.Domain;
using Banking.Account.Consumer.Infrastructure;
using Banking.Account.Consumer.Infrastructure.Persistence;
using Banking.Account.Consumer.Infrastructure.Repositories;
using Banking.Cqrs.Core.Events;
using EventBus.Base;
using EventBus.RabbitMQ;
using EventBus.RabbitMQ.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace Banking.Account.Consumer.RabbitMQ;

public static class ApplicationRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddRabbitMq(configuration);
        
        return services;
    }
    
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
         // SubscribeEvent 
        app.SubscribeRabbitMqEvent();
        app.MapGet("", async context => await context.Response.WriteAsync("RabbitMQ Consumer is up."));

        // Db Migration
        app.MigrateDatabase();
        return app;
    }
}