using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.DepositFunds;
using Banking.Cqrs.Core.Events;
using EventBus.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Events.FundsDeposited;

public class FundsDepositedEventHandler: IEventHandler<FundsDepositedEvent>
{
    private readonly ILogger<FundsDepositedEventHandler> _logger;
    private readonly IMediator _mediator;


    public FundsDepositedEventHandler(IMediator mediator, ILogger<FundsDepositedEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(FundsDepositedEvent @event, CancellationToken? cancellationToken = null)
    {
        var command = new DepositFundsCommand(@event.Id, @event.Amount);
        await _mediator.Send(command);
        
        _logger.LogInformation("Handling DepositFundsCommand: Identifier: ({@Identifier})", @event.Id);
        await Task.CompletedTask;
    }
}