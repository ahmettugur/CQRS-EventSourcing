using Banking.Account.Command.Application.Features.BankAccount.Commands.CloseAccount;
using Banking.Account.Command.Application.Features.BankAccount.Commands.OpenAccount;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Events;
using MediatR;

namespace Banking.Account.Command.Application.Aggregates;

public class AccountAggregate : AggregateRoot
{
    public bool Active { get; set; }
    public double Balance { get; set; }

    public AccountAggregate()
    {
    }

    public AccountAggregate(OpenAccountCommand command)
    {
        var accountOpenedEvent = new AccountOpenedEvent(command.Id, command.AccountHolder, command.AccountType, command.OpeningBalance);
        RaiseEvent(accountOpenedEvent);
    }

    public void Apply(AccountOpenedEvent @event)
    {
        Id = @event.Id;
        Active = true;
        Balance = @event.OpeningBalance;
    }

    public void DepositFunds(double amount)
    {
        if (!Active)
            throw new Exception("Funds cannot be deposited into a canceled account");
        if (amount <= 0)
            throw new Exception("Money deposit must be greater than zero");

        var fundsDepositEvent = new FundsDepositedEvent(Id)
        {
            Amount = amount
        };
        
        RaiseEvent(fundsDepositEvent);
    }

    public void Apply(FundsDepositedEvent @event)
    {
        Id = @event.Id;
        Active = true;
        Balance += @event.Amount;
    }
    
    public void WithDrawFunds(double amount)
    {
        if (!Active)
            throw new Exception("The bank account is closed");

        var fundsWithDrawnEvent = new FundsWithdrawnEvent(Id)
        {
            Amount = amount
        };
        
        RaiseEvent(fundsWithDrawnEvent);
    }
    
    public void Apply(FundsWithdrawnEvent @event)
    {
        Id = @event.Id;
        Balance -= @event.Amount;
    }

    public void CloseAccount()
    {
        if (!Active)
            throw new Exception("The account is closed");

        var accountClosedEvent = new AccountClosedEvent(Id);
        RaiseEvent(accountClosedEvent);
    }
    
    public void Apply(AccountClosedEvent @event)
    {
        Id = @event.Id;
        Active = false;
    }

}