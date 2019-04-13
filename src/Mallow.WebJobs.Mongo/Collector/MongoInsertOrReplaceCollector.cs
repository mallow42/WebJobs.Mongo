using System.Threading;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Mallow.Azure.WebJobs.Extensions.Mongo.Converters;
using Microsoft.Azure.WebJobs;
using MongoDB.Bson;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Collector
{
    internal class MongoInsertOrReplaceCollector<T> : IAsyncCollector<T>
    {
        private readonly IBsonCollection _collection;

        public MongoInsertOrReplaceCollector(IBsonCollection collection)
        {
            _collection = collection;
        }

        public Task AddAsync(T item, CancellationToken cancellationToken)
        {
            var bsonDocument = item.ToBsonDocument();
            var filter = FilterBuilder.CreateFilterByDocumentId(bsonDocument);
            bsonDocument.Remove(FilterBuilder.ID_FIELD);
            return _collection.UpsertOneAsync(filter, bsonDocument, cancellationToken);
        }

        public Task FlushAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}