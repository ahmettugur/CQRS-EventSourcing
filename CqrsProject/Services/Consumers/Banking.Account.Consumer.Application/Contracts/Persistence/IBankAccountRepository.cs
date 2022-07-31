using Banking.Account.Consumer.Domain;

namespace Banking.Account.Consumer.Application.Contracts.Persistence;

public interface IBankAccountRepository: IAsyncRepository<BankAccount>
{
    Task<BankAccount> FindByAccountIdentifier(string identifier);
    Task DeleteByIdentifier (string identifier);
    Task DepositBankAccountIdentifier (BankAccount bankAccount);
    Task WithDrawnBankAccountIdentifier (BankAccount bankAccount);
}