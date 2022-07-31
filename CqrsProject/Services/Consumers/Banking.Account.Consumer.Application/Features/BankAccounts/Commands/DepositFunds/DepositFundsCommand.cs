using MediatR;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Commands.DepositFunds;

public class DepositFundsCommand: IRequest
{
    public DepositFundsCommand(string identifier, double balance)
    {
        Identifier = identifier;
        Balance = balance;
    }

    public string Identifier { get;}
    public double Balance { get; }
}