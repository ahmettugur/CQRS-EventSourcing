using Banking.Account.Consumer.Domain.Common;

namespace Banking.Account.Consumer.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T: BaseDomainModel
{
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}