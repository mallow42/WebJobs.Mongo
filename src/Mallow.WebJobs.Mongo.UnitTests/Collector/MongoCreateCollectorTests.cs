using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo.Collector;
using Mallow.WebJobs.Mongo.UnitTests.Fakes;
using MongoDB.Bson;
using Xunit;

namespace Mallow.WebJobs.Mongo.UnitTests.Collector
{
    public class MongoCreateCollectorTests
    {
        private readonly MongoCreateCollector<TestDocument> _collector;
        private readonly CollectionFake _mongoCollection;

        public MongoCreateCollectorTests()
        {
            _mongoCollection = new CollectionFake();
            _collector = new MongoCreateCollector<TestDocument>(_mongoCollection);
        }

        [Fact]
        public async Task AddAsync_DoesntAddAnyDocument()
        {
            await _collector.AddAsync(new TestDocument("A", 1), CancellationToken.None);

            _mongoCollection.Documents.Should().BeEmpty();
        }

        [Fact]
        public async Task FlushAsync_InsertsAllDocumentsInBatch()
        {
            await _collector.AddAsync(new TestDocument("A", 1), CancellationToken.None);
            await _collector.AddAsync(new TestDocument("B", 2), CancellationToken.None);

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
        
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private class TestDocument
        {
            
            public string Name { get; }
            
            public int Id { get; }
            
            public TestDocument(string name, int id)
            {
                Name = name;
                Id = id;
            }
        }
    }
}