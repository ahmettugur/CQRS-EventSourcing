using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Banking.Account.Command.Domain.Common;

public interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }
    DateTime CreatedDate { get;}
    
}