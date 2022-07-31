using Banking.Account.Consumer.Application.Contracts.Persistence;
using Banking.Account.Consumer.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Commands.OpenAccount;

public class OpenAccountCommandHandler: IRequestHandler<OpenAccountCommand,BankAccount>
{
    private readonly ILogger<OpenAccountCommandHandler> _logger;
    private readonly IBankAccountRepository _bankAccountRepository;
    
    public OpenAccountCommandHandler(IServiceScopeFactory factory, ILogger<OpenAccountCommandHandler> logger)
    {
        _bankAccountRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IBankAccountRepository>();;
        _logger = logger;
    }

    public async Task<BankAccount> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
    {
        var bankAccount = new BankAccount
        {
            Identifier = request.Identifier,
            AccountHolder = request.AccountHolder,
            AccountType = request.AccountType,
            Balance = request.Balance,
            CreationDate = request.CreationDate
        };
        var result = await _bankAccountRepository.AddAsync(bankAccount);
        
        return result;
    }
}