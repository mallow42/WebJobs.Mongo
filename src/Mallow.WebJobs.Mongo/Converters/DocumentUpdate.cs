using MongoDB.Bson;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Converters
{
    internal class DocumentUpdate
    {
        public BsonDocument Update { get; }
        
        public object Id { get; }

        public DocumentUpdate(BsonDocument update, object id)
        {
            Update = update;
            Id = id;
        }
    }
}