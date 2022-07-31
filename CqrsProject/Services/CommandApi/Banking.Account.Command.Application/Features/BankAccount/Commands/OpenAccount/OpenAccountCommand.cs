using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.OpenAccount;

public class OpenAccountCommand: IRequest<bool>
{
    public string Id { get; set; } = string.Empty;
    public string AccountHolder { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public double OpeningBalance { get; set; }
}