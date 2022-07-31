using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.DepositFunds;
using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Account.Consumer.Application.Features.BankAccounts.Commands.WithdrawFunds;
using Banking.Account.Consumer.Kafka.Application.Features.BankAccounts.Events.ReplayAccount;
using Banking.Cqrs.Core.Events;
using EventBus.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Events.ReplayAccount
{
    public class ReplayAccountEventHandler : IEventHandler<ReplayAccountEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReplayAccountEventHandler> _logger;

        public ReplayAccountEventHandler(IMediator mediator, ILogger<ReplayAccountEventHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Handle(ReplayAccountEvent @event,CancellationToken? cancellationToken = null)
        {
            var jsonStr = JsonConvert.SerializeObject(@event.ReplayEvents);

            var events = JsonConvert.DeserializeObject<List<ReplayEventModel>>(jsonStr)?.OrderBy( _=> _.Version);


            foreach (var replayEvent in events!)
            {
                if (replayEvent.EventType == "AccountOpenedEvent")
                {
                    var accountOpenedEvent = JsonConvert.DeserializeObject<AccountOpenedEvent>(replayEvent.EventData?.ToString()!);
                    var accountCommand = new OpenAccountCommand(accountOpenedEvent!.Id, accountOpenedEvent.AccountHolder, accountOpenedEvent.AccountType, accountOpenedEvent.OpeningBalance, accountOpenedEvent.CreatedDate);
                    await _mediator.Send(accountCommand);
                }
                else if (replayEvent.EventType == "FundsDepositedEvent")
                {
                    var fundsDepositedEvent = JsonConvert.DeserializeObject<FundsDepositedEvent>(replayEvent.EventData?.ToString()!);
                    var command = new DepositFundsCommand(fundsDepositedEvent!.Id, fundsDepositedEvent.Amount);
                    await _mediator.Send(command);
                }
                else if (replayEvent.EventType == "FundsWithdrawnEvent")
                {
                    var fundsWithdrawnEvent = JsonConvert.DeserializeObject<FundsWithdrawnEvent>(replayEvent.EventData?.ToString()!);
                    var command = new WithdrawFundsCommand(fundsWithdrawnEvent!.Id, fundsWithdrawnEvent.Amount);
                    await _mediator.Send(command);
                }
            }

           await Task.CompletedTask;
        }
    }
}
