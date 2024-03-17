using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Ports.Repositories
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        string Id { get; set; }

        DateTime CreatedAt { get; }
    }
}
