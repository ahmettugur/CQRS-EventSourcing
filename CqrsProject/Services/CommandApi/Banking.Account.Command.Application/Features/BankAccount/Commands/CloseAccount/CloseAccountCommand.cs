using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.CloseAccount;

public class CloseAccountCommand: IRequest<bool>
{
    public string Id { get; set; } = string.Empty;
}