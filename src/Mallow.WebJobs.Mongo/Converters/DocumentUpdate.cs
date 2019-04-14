using MongoDB.Bson;
using MongoDB.Driver;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Converters
{
    internal class DocumentUpdate
    {
        public BsonDocument Update { get; }
        
        public FilterDefinition<BsonDocument> Filter { get; }

        public DocumentUpdate(BsonDocument update, FilterDefinition<BsonDocument> filter)
        {
            Update = update;
            Filter = filter;
        }
    }
}