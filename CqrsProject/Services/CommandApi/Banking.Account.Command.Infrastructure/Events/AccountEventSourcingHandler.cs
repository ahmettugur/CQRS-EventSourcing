using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Handlers;
using Banking.Cqrs.Core.Infrastructure;

namespace Banking.Account.Command.Infrastructure.Events;

public class AccountEventSourcingHandler: IEventSourcingHandler<AccountAggregate>
{
    private readonly IEventStore _eventStore;
    public AccountEventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }
    public async Task Save(AggregateRoot aggregate, string topic = "")
    {
        await _eventStore.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.GetVersion(),topic);
        aggregate.MarkChangesAsCommitted();
    }

    public async Task Replay(string aggregateId, string topic = "")
    {
        await _eventStore.SaveReplayEvents(aggregateId,topic);
    }

    public async Task<AccountAggregate> GetById(string id)
    {
        var aggregate = new AccountAggregate();
        var events = await _eventStore.GetEvents(id);
        if (!events.Any()) 
            return aggregate;
        
        aggregate.ReplayEvents(events!);
        var lastVersion = events.Max(e => e!.Version);
        aggregate.SetVersion(lastVersion);

        return aggregate;
    }
}