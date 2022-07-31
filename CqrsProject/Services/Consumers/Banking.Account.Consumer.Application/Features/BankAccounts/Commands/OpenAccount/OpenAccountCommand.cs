using Banking.Account.Consumer.Domain;
using MediatR;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Commands.OpenAccount;

public class OpenAccountCommand: IRequest<BankAccount>
{
    public string Identifier { get; }
    public string AccountHolder { get; } 
    public DateTime CreationDate { get;}
    public string AccountType { get; }
    public double Balance { get; }

    public OpenAccountCommand(string identifier, string accountHolder, string accountType, double balance, DateTime creationDate)
    {
        Identifier = identifier;
        AccountHolder = accountHolder;
        CreationDate = creationDate;
        AccountType = accountType;
        Balance = balance;
    }
}