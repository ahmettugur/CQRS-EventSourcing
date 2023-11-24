using Banking.Account.Consumer.Application.Contracts.Persistence;
using Banking.Account.Consumer.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Commands.DepositFunds;

public class DepositFundsCommandHandler: IRequestHandler<DepositFundsCommand>
{
    private readonly ILogger<DepositFundsCommandHandler> _logger;
    private readonly IBankAccountRepository _bankAccountRepository;
    
    public DepositFundsCommandHandler(IServiceScopeFactory factory , ILogger<DepositFundsCommandHandler> logger)
    {
        _bankAccountRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IBankAccountRepository>();
        _logger = logger;
    }
    
    public async Task Handle(DepositFundsCommand request, CancellationToken cancellationToken)
    {
        var bankAccount = new BankAccount
        {
            Identifier = request.Identifier,
            Balance = request.Balance
        };
        await _bankAccountRepository.DepositBankAccountIdentifier(bankAccount);
    }
}