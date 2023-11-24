using Banking.Account.Consumer.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Banking.Account.Consumer.Application.Features.BankAccounts.Commands.CloseAccount;

public class CloseAccountCommandHandler: IRequestHandler<CloseAccountCommand>
{
    private readonly ILogger<CloseAccountCommandHandler> _logger;
    private readonly IBankAccountRepository _bankAccountRepository;
    
    public CloseAccountCommandHandler(IServiceScopeFactory factory , ILogger<CloseAccountCommandHandler> logger)
    {
        _bankAccountRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IBankAccountRepository>();;
        _logger = logger;
    }
    
    public async Task Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        await _bankAccountRepository.DeleteByIdentifier(request.Identifier);
    }
}