using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Microsoft.Azure.WebJobs;
using MongoDB.Bson.Serialization;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Converters
{
    internal class MongoEnumerableConverter<T> : IAsyncConverter<MongoAttribute, IEnumerable<T>>
    {
        private readonly IMongoCollectionFactory _mongoCollectionFactory;

        public MongoEnumerableConverter(IMongoCollectionFactory mongoCollectionFactory)
        {
            _mongoCollectionFactory = mongoCollectionFactory;
        }

        public async Task<IEnumerable<T>> ConvertAsync(MongoAttribute input, CancellationToken cancellationToken)
        {
            var collection = _mongoCollectionFactory.Create(input.ToConnectionSettings());
            var documents = await collection.FindAsync(input.Filter, cancellationToken);

            return documents.Select(doc => BsonSerializer.Deserialize<T>(doc)).ToList();
        }
    }
}