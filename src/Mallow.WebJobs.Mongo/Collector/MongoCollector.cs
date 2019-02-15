using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Microsoft.Azure.WebJobs;
using MongoDB.Bson;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Collector
{
    internal class MongoCollector<T> : IAsyncCollector<T>
    {
        private readonly ConcurrentQueue<BsonDocument> _documents = new ConcurrentQueue<BsonDocument>();
        private readonly IBsonCollection _collection;

            public MongoCollector(IBsonCollection collection)
        {
            _collection = collection;
        }

        public Task AddAsync(T item, CancellationToken cancellationToken)
        {
            _documents.Enqueue(item.ToBsonDocument());
            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken)
        {
            return _collection.InsertManyAsync(_documents, cancellationToken);
        }
    }
}