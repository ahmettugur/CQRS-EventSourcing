using Banking.Account.Consumer.Application.Contracts.Persistence;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.AccountClosed;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.AccountOpened;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.FundsDeposited;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.FundsWithdraw;
using Banking.Account.Consumer.Application.Features.BankAccounts.Events.ReplayAccount;
using Banking.Account.Consumer.Infrastructure.Persistence;
using Banking.Account.Consumer.Infrastructure.Repositories;
using Banking.Cqrs.Core.Events;
using EventBus.Kafka;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.OpenAccount;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Banking.Account.Consumer.Infrastructure;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        services.AddDbContext<BankAccountDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        return services;
    }
    
    public static void MigrateDatabase(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<BankAccountDbContext>();
        dataContext.Database.Migrate();
    }
}