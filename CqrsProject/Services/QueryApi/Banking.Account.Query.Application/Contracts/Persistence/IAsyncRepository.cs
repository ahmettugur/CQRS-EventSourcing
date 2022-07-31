using Banking.Account.Query.Domain.Common;

namespace Banking.Account.Query.Application.Contracts.Persistence;

public interface IAsyncRepository<T> where T: BaseDomainModel
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}