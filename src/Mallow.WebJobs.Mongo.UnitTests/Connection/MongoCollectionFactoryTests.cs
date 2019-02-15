using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Xunit;

namespace Mallow.WebJobs.Mongo.UnitTests.Connection
{
    public class MongoCollectionFactoryTests
    {
        private const string CONNECTION_STRING = "mongodb://my-mongo-instance:2805";
        private const string DATABASE_ID = "DatabaseA";
        private const string COLLECTION_ID = "CollectionA";
        
        private readonly ConnectionSettings _connectionSettings =
            new ConnectionSettings(CONNECTION_STRING, DATABASE_ID, COLLECTION_ID);

        [Fact]
        public void Create_CachesCollection()
        {
            var factory = new MongoCollectionFactory();

            var collection1 = factory.Create(_connectionSettings);
            var collection2 = factory.Create(_connectionSettings);

            collection1.Should().Be(collection2);
        }
    }
}