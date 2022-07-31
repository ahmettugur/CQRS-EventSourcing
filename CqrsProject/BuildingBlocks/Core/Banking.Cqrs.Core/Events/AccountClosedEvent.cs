using Banking.Cqrs.Core.Contracts;

namespace Banking.Cqrs.Core.Events;

[Event("AccountClosedEvent")]
public class AccountClosedEvent: BaseEvent
{
    public AccountClosedEvent(string id) : base(id)
    {
    }
}