using Banking.Account.Consumer.Application.Contracts.Persistence;
using Banking.Account.Consumer.Domain.Common;
using Banking.Account.Consumer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Account.Consumer.Infrastructure.Repositories;

public class GenericRepository<T>: IAsyncRepository<T> where T: BaseDomainModel
{
    private readonly BankAccountDbContext _context;

    protected GenericRepository(BankAccountDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}