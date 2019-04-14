using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo.Converters;
using Mallow.WebJobs.Mongo.UnitTests.Base;
using Mallow.WebJobs.Mongo.UnitTests.Fakes;
using MongoDB.Bson;
using Xunit;

namespace Mallow.WebJobs.Mongo.UnitTests.Converters
{
    public class MongoDocumentConverterTests
    {
        private const string OBJECT_ID = "5bf709e63c5b4c3da8f44202";
        
        private readonly CollectionFake _collectionFake;
        private readonly MongoCollectionFactoryFake _mongoCollectionFactory;

        public MongoDocumentConverterTests()
        {
            _collectionFake = new CollectionFake();
            _mongoCollectionFactory = new MongoCollectionFactoryFake(_collectionFake);
        }

        [Fact]
        public async Task ConvertAsync_MongoAttributeWithExistingObjectId_ReturnsCorrectDocument()
        {
            var mongoAttribute = MongoAttributeFactory.CreateWithId(OBJECT_ID);
            var converter = new MongoDocumentConverter<TestDocumentWithId<ObjectId>>(_mongoCollectionFactory);
            var document = new TestDocumentWithId<ObjectId>("A", ObjectId.Parse(OBJECT_ID));
            _collectionFake.AddDocument(document);

            var result = await converter.ConvertAsync(mongoAttribute, CancellationToken.None);
            
            result.Should().BeEquivalentTo(document);
        }

        [Fact]
        public async Task ConvertAsync_MongoAttributeWithExistingId_ReturnsCorrectDocument()
        {
            var mongoAttribute = MongoAttributeFactory.CreateWithId("id256");
            var converter = new MongoDocumentConverter<TestDocumentWithId<string>>(_mongoCollectionFactory);
            var document = new TestDocumentWithId<string>("A", "id256");
            _collectionFake.AddDocument(document);
            

            var result = await converter.ConvertAsync(mongoAttribute, CancellationToken.None);
            
            result.Should().BeEquivalentTo(document);
        }

        [Fact]
        public async Task ConvertAsync_MongoAttributeWithNonExistingId_ReturnsNull()
        {
            var mongoAttribute = MongoAttributeFactory.CreateWithId(OBJECT_ID);
            var converter = new MongoDocumentConverter<TestDocumentWithId<ObjectId>>(_mongoCollectionFactory);

            var result = await converter.ConvertAsync(mongoAttribute, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}