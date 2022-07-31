using Banking.Cqrs.Core.Contracts;

namespace Banking.Cqrs.Core.Events;

[Event("FundsDepositedEvent")]
public class FundsDepositedEvent: BaseEvent
{
    public double Amount { get; set; }
    public FundsDepositedEvent(string id) : base(id)
    {
    }
}