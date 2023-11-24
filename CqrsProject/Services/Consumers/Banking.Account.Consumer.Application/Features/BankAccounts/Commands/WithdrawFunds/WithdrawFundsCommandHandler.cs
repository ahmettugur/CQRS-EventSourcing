using Banking.Account.Consumer.Application.Contracts.Persistence;
using Banking.Account.Consumer.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Commands.WithdrawFunds;

public class WithdrawFundsCommandHandler: IRequestHandler<WithdrawFundsCommand>
{
    private readonly ILogger<WithdrawFundsCommandHandler> _logger;
    private readonly IBankAccountRepository _bankAccountRepository;
    
    public WithdrawFundsCommandHandler(IServiceScopeFactory factory , ILogger<WithdrawFundsCommandHandler> logger)
    {
        _bankAccountRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IBankAccountRepository>();;
        _logger = logger;
    }

    public async Task Handle(WithdrawFundsCommand request, CancellationToken cancellationToken)
    {
        var bankAccount = new BankAccount
        {
            Identifier = request.Identifier,
            Balance = request.Balance
        };
        await _bankAccountRepository.WithDrawnBankAccountIdentifier(bankAccount);
    }
}