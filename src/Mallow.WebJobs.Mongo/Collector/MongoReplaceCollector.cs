using System.Threading;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Mallow.Azure.WebJobs.Extensions.Mongo.Converters;
using Microsoft.Azure.WebJobs;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Collector
{
    internal class MongoReplaceCollector<T> : IAsyncCollector<T>
    {
        private readonly IBsonCollection _collection;

        public MongoReplaceCollector(IBsonCollection collection)
        {
            _collection = collection;
        }

        public Task AddAsync(T item, CancellationToken cancellationToken)
        {
            var update = UpdateBuilder.CreateUpdate(item);
            return _collection.ReplaceOneAsync(update.Filter, update.Update, cancellationToken);
        }

        public Task FlushAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}