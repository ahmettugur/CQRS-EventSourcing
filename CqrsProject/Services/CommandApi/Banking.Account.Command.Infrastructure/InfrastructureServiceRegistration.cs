using Banking.Account.Command.Application.Aggregates;
using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Infrastructure.Events;
using Banking.Account.Command.Infrastructure.Repositories;
using Banking.Cqrs.Core.Events;
using Banking.Cqrs.Core.Handlers;
using Banking.Cqrs.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace Banking.Account.Command.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        BsonClassMap.RegisterClassMap<BaseEvent>();
        BsonClassMap.RegisterClassMap<AccountOpenedEvent>();
        BsonClassMap.RegisterClassMap<AccountClosedEvent>();
        BsonClassMap.RegisterClassMap<FundsDepositedEvent>();
        BsonClassMap.RegisterClassMap<FundsWithdrawnEvent>();

        services.AddScoped(typeof(IMongoRepository<>),typeof(MongoRepository<>));
        services.AddTransient<IEventStoreRepository, EventStoreRepository>();
        services.AddTransient<IReplayEventStoreRepository, ReplayEventStoreRepository>();
        services.AddTransient<IEventStore, AccountEventStore>();
        services.AddTransient<IEventSourcingHandler<AccountAggregate>, AccountEventSourcingHandler>();
        
        return services;
    }
}