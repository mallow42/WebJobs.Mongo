using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;

namespace Mallow.WebJobs.Mongo.UnitTests.Fakes
{
    internal class MongoCollectionFactoryFake : IMongoCollectionFactory
    {
        private readonly IBsonCollection _collection;

        public MongoCollectionFactoryFake(IBsonCollection collection)
        {
            _collection = collection;
        }

        public IBsonCollection Create(ConnectionSettings connectionSettings)
        {
            return _collection;
        }
    }
}