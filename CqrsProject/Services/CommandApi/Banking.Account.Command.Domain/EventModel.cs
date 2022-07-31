using Banking.Account.Command.Domain.Common;
using Banking.Cqrs.Core.Events;
using MongoDB.Bson.Serialization.Attributes;

namespace Banking.Account.Command.Domain;

[BsonCollection("eventStore")]
public class EventModel: BaseEventModel<BaseEvent>
{

}