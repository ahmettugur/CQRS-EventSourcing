using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.ReplayAccount;

public class ReplayByAggregateIdentifierCommand: IRequest<bool>
{
    public string AggregateId { get; set; } = string.Empty;
}