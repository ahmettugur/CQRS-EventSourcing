using Banking.Account.Command.Domain;

namespace Banking.Account.Command.Application.Contracts.Persistence;

public interface IReplayEventStoreRepository: IMongoRepository<ReplayEventModel>
{
    Task<IEnumerable<ReplayEventModel>> FindByAggregateIdentifier(string aggregateIdentifier);
}