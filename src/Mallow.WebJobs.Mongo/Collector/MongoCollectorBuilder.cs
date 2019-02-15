using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Microsoft.Azure.WebJobs;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Collector
{
    internal class MongoCollectorBuilder<T> : IConverter<MongoAttribute, IAsyncCollector<T>>
    {
        private readonly IMongoCollectionFactory _factory;

        public MongoCollectorBuilder(IMongoCollectionFactory factory)
        {
            _factory = factory;
        }

        public IAsyncCollector<T> Convert(MongoAttribute attribute)
        {
            var collection = _factory.Create(attribute.ToConnectionSettings());
            return new MongoCollector<T>(collection);
        }
    }
}