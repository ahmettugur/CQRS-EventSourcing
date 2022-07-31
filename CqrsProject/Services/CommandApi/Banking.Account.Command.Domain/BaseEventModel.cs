using Banking.Account.Command.Domain.Common;
using Banking.Cqrs.Core.Events;
using MongoDB.Bson.Serialization.Attributes;

namespace Banking.Account.Command.Domain;

public class BaseEventModel<T> : Document
{
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }

    [BsonElement("aggregateIdentifier")]
    public string AggregateIdentifier { get; set; } = string.Empty;

    [BsonElement("aggregateType")]
    public string AggregateType { get; set; } = string.Empty;

    [BsonElement("version")]
    public int Version { get; set; }

    [BsonElement("eventType")]
    public string EventType { get; set; } = string.Empty;

    [BsonElement("eventData")]
    public T? EventData { get; set; }
}