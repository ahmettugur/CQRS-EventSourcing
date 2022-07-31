using Banking.Account.Consumer.Application.Contracts.Persistence;
using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Account.Consumer.Domain;
using Banking.Cqrs.Core.Events;
using EventBus.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Events.AccountOpened;

public class AccountOpenedEventHandler:IEventHandler<AccountOpenedEvent>
{
    private readonly ILogger<AccountOpenedEventHandler> _logger;
    private readonly IMediator _mediator;
    public AccountOpenedEventHandler(IMediator mediator,ILogger<AccountOpenedEventHandler> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(AccountOpenedEvent @event, CancellationToken? cancellationToken = null)
    {
        var accountCommand = new OpenAccountCommand(@event.Id, @event.AccountHolder, @event.AccountType, @event.OpeningBalance, @event.CreatedDate);
        await _mediator.Send(accountCommand);

        _logger.LogInformation("Handling AccountOpenedEvent: Identifier: ({@Identifier})", @event.Id);
        await Task.CompletedTask;
    }
}