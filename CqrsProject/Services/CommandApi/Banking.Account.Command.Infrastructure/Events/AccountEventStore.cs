using Banking.Account.Command.Application.Aggregates;
using Banking.Account.Command.Application.Contracts.Persistence;
using Banking.Account.Command.Domain;
using Banking.Cqrs.Core.Events;
using Banking.Cqrs.Core.Infrastructure;
using EventBus.Base;
using EventBus.RabbitMQ.Abstraction;

namespace Banking.Account.Command.Infrastructure.Events;

public class AccountEventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly IReplayEventStoreRepository _replayEventStoreRepository;
    private readonly IBaseEventBus _eventProducer;

    public AccountEventStore(IEventStoreRepository eventStoreRepository, IBaseEventBus eventProducer, IReplayEventStoreRepository replayEventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
        _eventProducer = eventProducer;
        _replayEventStoreRepository = replayEventStoreRepository;
    }

    public async Task SaveEvents(string aggregateId, IEnumerable<BaseEvent> events, int expectedVersion,string topic="")
    {
        var eventStream = await _eventStoreRepository.FindByAggregateIdentifier(aggregateId);
        var eventModels = eventStream.ToList();
        if (expectedVersion != -1 && eventModels.ElementAt(eventModels.Count - 1).Version != expectedVersion)
        {
            throw new Exception("Concurrency errors");
        }

        var version = expectedVersion;
        foreach (var evt in events)
        {
            version++;
            evt.Version = version;
            var eventModel = new EventModel
            {
                Timestamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(AccountAggregate),
                Version = version,
                EventType = evt.GetType().Name,
                EventData = evt
            };
            await _eventStoreRepository.InsertDocument(eventModel);
            _eventProducer.Publish(evt,topic);
        }
    }

    public async Task SaveReplayEvents(string aggregateId, string topic = "")
    {
        var events = await _eventStoreRepository.FindByAggregateIdentifier(aggregateId);
        if (!events.Any())
        {
            return;
        }

        events = events.OrderBy(_ => _.Version);

        var replyaAccountEvent = new ReplayAccountEvent();
        foreach (var eventModel in events)
        {
            var replayEventModel = new ReplayEventModel
            {
                Timestamp = DateTime.Now,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(AccountAggregate),
                Version = eventModel.Version,
                EventType = eventModel.EventData!.GetType().Name,
                EventData = eventModel.EventData
            };
            await _replayEventStoreRepository.InsertDocument(replayEventModel);
            replyaAccountEvent.ReplayEvents.Add(replayEventModel);
        }
        _eventProducer.Publish(replyaAccountEvent,topic);
    }

    public async Task<List<BaseEvent?>> GetEvents(string aggregateId)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateIdentifier(aggregateId);
        var eventModels = eventStream.ToList();
        if (!eventModels.Any())
        {
            throw new Exception("The bank account does not exist");
        }

        return eventModels.Select(x => x.EventData).ToList();
    }
}