using Banking.Cqrs.Core.Contracts;

namespace Banking.Cqrs.Core.Events;

[Event("AccountOpenedEvent")]
public class AccountOpenedEvent: BaseEvent
{
    public string AccountHolder { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public double OpeningBalance { get; set; }
    
    public AccountOpenedEvent(string id,string accountHolder,string accountType, double openingBalance): base(id)
    {
        AccountType = accountType;
        OpeningBalance = openingBalance;
        AccountHolder = accountHolder;
    }
}