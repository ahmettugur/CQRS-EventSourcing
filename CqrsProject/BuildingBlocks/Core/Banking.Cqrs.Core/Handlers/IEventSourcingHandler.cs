using Banking.Cqrs.Core.Domain;

namespace Banking.Cqrs.Core.Handlers;

public interface IEventSourcingHandler<T>
{
    Task Save(AggregateRoot aggregate,string topic = "");
    Task Replay(string aggregateId, string topic = "");
    Task<T> GetById(string id);
}