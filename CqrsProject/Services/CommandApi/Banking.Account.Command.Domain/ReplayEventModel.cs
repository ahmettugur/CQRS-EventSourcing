using Banking.Account.Command.Domain.Common;

namespace Banking.Account.Command.Domain;

[BsonCollection("replayEventStore")]
public class ReplayEventModel: BaseEventModel<object>
{
}