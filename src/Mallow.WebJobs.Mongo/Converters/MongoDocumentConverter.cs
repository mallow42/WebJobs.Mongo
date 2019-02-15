using System.Threading;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Microsoft.Azure.WebJobs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Converters
{
    internal class MongoDocumentConverter<T> : IAsyncConverter<MongoAttribute, T>
    {
        private readonly IMongoCollectionFactory _mongoCollectionFactory;

        public MongoDocumentConverter(IMongoCollectionFactory mongoCollectionFactory)
        {
            _mongoCollectionFactory = mongoCollectionFactory;
        }

        public async Task<T> ConvertAsync(MongoAttribute input, CancellationToken cancellationToken)
        {
            var collection = _mongoCollectionFactory.Create(input.ToConnectionSettings());
            var filter = CreateFilter(input.Id);
            var document = await collection.FindOneOrDefaultAsync(filter, cancellationToken);
            if (document == null)
            {
                return default(T);
            }
            return BsonSerializer.Deserialize<T>(document);
        }
        
        private static FilterDefinition<BsonDocument> CreateFilter(string id)
        {
            return Builders<BsonDocument>.Filter.Eq("_id", GetId(id));
        }

        private static object GetId(string id)
        {
            if(ObjectId.TryParse(id, out var objectId))
            {
                return objectId;
            }

            return id;
        }
    }
}