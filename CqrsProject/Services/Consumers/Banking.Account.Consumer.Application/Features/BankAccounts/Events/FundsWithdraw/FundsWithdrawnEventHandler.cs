using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.WithdrawFunds;
using Banking.Cqrs.Core.Events;
using EventBus.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Events.FundsWithdraw;

public class FundsWithdrawnEventHandler: IEventHandler<FundsWithdrawnEvent>
{
    private readonly ILogger<FundsWithdrawnEventHandler> _logger;
    private readonly IMediator _mediator;

    public FundsWithdrawnEventHandler(IMediator mediator, ILogger<FundsWithdrawnEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(FundsWithdrawnEvent @event, CancellationToken? cancellationToken = null)
    {
        var command = new WithdrawFundsCommand(@event.Id,@event.Amount);
        await _mediator.Send(command);
        
        _logger.LogInformation("Handling WithdrawFundsCommand: Identifier: ({@Identifier})", @event.Id);
        await Task.CompletedTask;
    }
}