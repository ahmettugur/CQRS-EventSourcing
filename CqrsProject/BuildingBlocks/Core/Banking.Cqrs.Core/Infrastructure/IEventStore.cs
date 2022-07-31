using System.Reflection;
using Banking.Cqrs.Core.Events;

namespace Banking.Cqrs.Core.Infrastructure;

public interface IEventStore
{
    public Task SaveEvents(string aggregateId, IEnumerable<BaseEvent> events, int expectedVersion, string topic = "");
    public Task SaveReplayEvents(string aggregateId, string topic = "");
    public Task<List<BaseEvent?>> GetEvents(string aggregateId);
}