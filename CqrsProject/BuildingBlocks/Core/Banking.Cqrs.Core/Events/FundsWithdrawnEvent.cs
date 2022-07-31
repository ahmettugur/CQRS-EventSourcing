using Banking.Cqrs.Core.Contracts;

namespace Banking.Cqrs.Core.Events;

[Event("FundsWithdrawnEvent")]
public class FundsWithdrawnEvent: BaseEvent
{
    public double Amount { get; set; }
    public FundsWithdrawnEvent(string id) : base(id)
    {
    }
}
