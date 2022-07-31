using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handlers;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.DepositFunds;

public class DepositFundsCommandHandler: IRequestHandler<DepositFundsCommand,bool>
{
    private readonly IEventSourcingHandler<AccountAggregate> _eventSourcingHandler;
    public DepositFundsCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }
    public async Task<bool> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
    {
        var aggregate = await _eventSourcingHandler.GetById(request.Id);
        aggregate.DepositFunds(request.Amount);
        await _eventSourcingHandler.Save(aggregate, "FundsDepositedEvent");
        return true;
    }
}