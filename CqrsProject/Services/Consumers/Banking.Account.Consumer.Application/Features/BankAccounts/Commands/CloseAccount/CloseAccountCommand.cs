using MediatR;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Commands.CloseAccount;

public class CloseAccountCommand: IRequest
{
    public CloseAccountCommand(string identifier)
    {
        Identifier = identifier;
    }

    public string Identifier { get; }
}