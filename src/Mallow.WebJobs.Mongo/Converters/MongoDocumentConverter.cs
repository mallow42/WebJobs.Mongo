using System.Threading;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Microsoft.Azure.WebJobs;
using MongoDB.Bson.Serialization;

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
            var filter = FilterBuilder.CreateFilter(input.Id);
            var document = await collection.FindOneOrDefaultAsync(filter, cancellationToken);
            if (document == null)
            {
                return default(T);
            }
            return BsonSerializer.Deserialize<T>(document);
        }
    }
}