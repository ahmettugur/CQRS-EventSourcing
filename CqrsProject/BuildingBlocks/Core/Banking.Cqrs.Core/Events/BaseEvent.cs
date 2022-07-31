using Banking.Cqrs.Core.Messages;
using EventBus.Base;

namespace Banking.Cqrs.Core.Events;

public class BaseEvent: Event
{
    public int Version { get; set; }
    public string Id { get; set; }

    public BaseEvent(string id)
    {
        Id = id;
    }

}