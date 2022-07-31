using Banking.Account.Query.Domain;

namespace Banking.Account.Query.Application.Contracts.Persistence;

public interface IBankAccountRepository: IAsyncRepository<BankAccount>
{
    Task<BankAccount> FindByAccountIdentifier(string identifier);
    Task DeleteByIdentifier (string identifier);
    Task DepositBankAccountIdentifier (BankAccount bankAccount);
    Task WithDrawnBankAccountIdentifier (BankAccount bankAccount);
}