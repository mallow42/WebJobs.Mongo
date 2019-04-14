using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo.Collector;
using Mallow.WebJobs.Mongo.UnitTests.Base;
using Mallow.WebJobs.Mongo.UnitTests.Converters;
using Mallow.WebJobs.Mongo.UnitTests.Fakes;
using MongoDB.Bson;
using Xunit;

namespace Mallow.WebJobs.Mongo.UnitTests.Collector
{
    public class MongoCreateCollectorTests
    {
        private readonly MongoCreateCollector<TestDocumentWithId<int>> _collector;
        private readonly CollectionFake _mongoCollection;

        public MongoCreateCollectorTests()
        {
            _mongoCollection = new CollectionFake();
            _collector = new MongoCreateCollector<TestDocumentWithId<int>>(_mongoCollection);
        }

        [Fact]
        public async Task AddAsync_DoesntAddAnyDocument()
        {
            await _collector.AddAsync(new TestDocumentWithId<int>("A", 1), CancellationToken.None);

            _mongoCollection.Documents.Should().BeEmpty();
        }

        [Fact]
        public async Task FlushAsync_InsertsAllDocumentsInBatch()
        {
            await _collector.AddAsync(new TestDocumentWithId<int>("A", 1), CancellationToken.None);
            await _collector.AddAsync(new TestDocumentWithId<int>("B", 2), CancellationToken.None);

            await _collector.FlushAsync(CancellationToken.None);

            var docA = new BsonDocument()
            {
                {"Name", "A"},
                {"_id", 1}
            };
            var docB = new BsonDocument()
            {
                {"Name", "B"},
                {"_id", 2}
            };
            _mongoCollection.Documents.Should().BeEquivalentTo(docA, docB);
        }
    }
}