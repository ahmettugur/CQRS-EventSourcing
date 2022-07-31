using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using Banking.Account.Query.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Account.Query.Infrastructure.Repositories;

public class BankAccountRepository: RepositoryBase<BankAccount>,IBankAccountRepository
{
    
    public BankAccountRepository(BankAccountDbContext context) : base(context)
    {
    }

    public async Task<BankAccount> FindByAccountIdentifier(string identifier)
    {
        return (await _context.BankAccounts!.Where(x => x.Identifier == identifier).FirstOrDefaultAsync())!;
    }

    public async Task DeleteByIdentifier(string identifier)
    {
        var bankAccount = await _context.BankAccounts!.Where(x => x.Identifier == identifier).FirstOrDefaultAsync();
        if (bankAccount == null)
            throw new Exception($"Could not delete bank account with id {identifier}");

        _context.BankAccounts!.Remove(bankAccount);
        await _context.SaveChangesAsync();
    }

    public async Task DepositBankAccountIdentifier(BankAccount bankAccount)
    {
        var account = await _context.BankAccounts!.Where(x => x.Identifier == bankAccount.Identifier).FirstOrDefaultAsync();
        if (account == null)
            throw new Exception($"The bank account with identifier could not be updated {bankAccount.Identifier}");

        account.Balance += bankAccount.Balance;

        await UpdateAsync(account);

    }

    public async Task WithDrawnBankAccountIdentifier(BankAccount bankAccount)
    {
        var account = await _context.BankAccounts!.Where(x => x.Identifier == bankAccount.Identifier).FirstOrDefaultAsync();
        if (account == null)
            throw new Exception($"The bank account with identifier could not be updated {bankAccount.Identifier}");

        if(account.Balance < bankAccount.Balance)
            throw new Exception($"The account balance is less than the money you want to withdraw for {bankAccount.Id}");
        
        account.Balance -= bankAccount.Balance;

        await UpdateAsync(account);
    }
}