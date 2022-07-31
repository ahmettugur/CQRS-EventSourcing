using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.WithdrawFunds;

public class WithdrawFundsCommand: IRequest<bool>
{
    public string Id { get;}
    public double Amount { get;}
    
    public WithdrawFundsCommand(string id, double amount)
    {
        Id = id;
        Amount = amount;
    }
}