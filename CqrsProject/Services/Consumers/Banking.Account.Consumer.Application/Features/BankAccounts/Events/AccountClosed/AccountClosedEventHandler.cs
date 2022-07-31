using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.CloseAccount;
using Banking.Cqrs.Core.Events;
using EventBus.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Events.AccountClosed;

public class AccountClosedEventHandler: IEventHandler<AccountClosedEvent>
{
    private readonly ILogger<AccountClosedEventHandler> _logger;
    private readonly IMediator _mediator;

    public AccountClosedEventHandler(IMediator mediator,ILogger<AccountClosedEventHandler> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(AccountClosedEvent @event, CancellationToken? cancellationToken = null)
    {
        var command = new CloseAccountCommand(@event.Id);
        await _mediator.Send(command);
        _logger.LogInformation("Handling AccountClosedEvent: Identifier: ({@Identifier})", @event.Id);
        await Task.CompletedTask;

    }
}