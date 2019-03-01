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
    public class MongoEnumerableConverterTests
    {
        private readonly CollectionFake _collectionFake;
        private readonly MongoEnumerableConverter<TestDocument> _converter;

        public MongoEnumerableConverterTests()
        {
            _collectionFake = new CollectionFake();
            _converter = new MongoEnumerableConverter<TestDocument>(new MongoCollectionFactoryFake(_collectionFake));
        }

        [Fact]
        public async Task ConvertAsync_FilterNoDocuments_ReturnsEmptyCollection()
        {
            var mongoAttribute = MongoAttributeFactory.CreateWithFilter("{\"Name\" : \"A\"}");
            
            var result = await _converter.ConvertAsync(mongoAttribute, CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ConvertAsync_FilterMultipleDocuments_ReturnsAllDocuments()
        {
            var mongoAttribute = MongoAttributeFactory.CreateWithFilter("{\"Name\" : \"A\"}");
            var document = new TestDocument()
            {
                Name = "A"
            };
            _collectionFake.AddDocuments(mongoAttribute.Filter, document, document);

            var result = await _converter.ConvertAsync(mongoAttribute, CancellationToken.None);

            result.Should().BeEquivalentTo(document, document);
        }

        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable once UnusedMember.Local
        private class TestDocument
        {
            
            public ObjectId Id { get; set; }
            
            public string Name { get; set; }
        }
    }
}