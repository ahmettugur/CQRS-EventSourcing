using MediatR;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Commands.WithdrawFunds;

public class WithdrawFundsCommand: IRequest
{
    public WithdrawFundsCommand(string identifier, double balance)
    {
        Identifier = identifier;
        Balance = balance;
    }

    public string Identifier { get;}
    public double Balance { get; }
}