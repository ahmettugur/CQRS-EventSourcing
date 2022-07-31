using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handlers;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccount.Commands.CloseAccount;

public class CloseAccountCommandHandler: IRequestHandler<CloseAccountCommand,bool>
{
    private readonly IEventSourcingHandler<AccountAggregate> _eventSourcingHandler;

    public CloseAccountCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task<bool> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        var aggregate = await _eventSourcingHandler.GetById(request.Id);
        aggregate.CloseAccount();
        await _eventSourcingHandler.Save(aggregate, "AccountClosedEvent");
        return true;
    }
}