using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.DepositFunds;

public class DepositFundsCommand: IRequest<bool>
{
    public string Id { get;}
    public double Amount { get;}
    
    public DepositFundsCommand(string id, double amount)
    {
        Id = id;
        Amount = amount;
    }
}