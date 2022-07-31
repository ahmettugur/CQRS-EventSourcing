using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.OpenAccount;

public class OpenAccountCommand: IRequest<bool>
{
    public string Id { get;}
    public string AccountHolder { get; }
    public string AccountType { get;  }
    public double OpeningBalance { get; }

    public OpenAccountCommand(string accountHolder, string accountType, double openingBalance)
    {
        Id = Guid.NewGuid().ToString();
        AccountHolder = accountHolder;
        AccountType = accountType;
        OpeningBalance = openingBalance;
    }
}