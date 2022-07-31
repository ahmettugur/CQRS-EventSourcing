using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handlers;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.ReplayAccount;

public class ReplayByAggregateIdentifierCommandHandler: IRequestHandler<ReplayByAggregateIdentifierCommand,bool>
{
    private readonly IEventSourcingHandler<AccountAggregate> _eventSourcingHandler;


    public ReplayByAggregateIdentifierCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task<bool> Handle(ReplayByAggregateIdentifierCommand request, CancellationToken cancellationToken)
    {
        await _eventSourcingHandler.Replay(request.AggregateId, "ReplayAccountEvent");
        return true;
    }
}