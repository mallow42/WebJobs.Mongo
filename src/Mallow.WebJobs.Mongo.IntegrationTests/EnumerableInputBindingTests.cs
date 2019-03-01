using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.WebJobs.Mongo.IntegrationTests.Base;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Mallow.WebJobs.Mongo.IntegrationTests
{
    public class EnumerableInputBindingTests : IDisposable
    {
        private readonly FunctionsApi _functionsApi;
        private readonly MongoFixture _mongoFixture;

        public EnumerableInputBindingTests()
        {
            _mongoFixture = new MongoFixture();
            _functionsApi = new FunctionsApi();
        }

        [Fact]
        public async Task GetDocuments_WithNameFilter_ReturnsCorrectDocument()
        {
            var collection = _mongoFixture.CreateCollection<TestDocument>();
            var documentA = new TestDocument("A", 1);
            var documentB = new TestDocument("B", 2);
            await collection.InsertOneAsync(documentA);
            await collection.InsertOneAsync(documentB);
            var filter = new JObject()
            {
                {"Name", "A"}
            }.ToString();

            var result = await _functionsApi.GetDocuments<TestDocument>(_mongoFixture.GetCollectionName(), filter);

            result.IsSuccessStatusCode.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(documentA);
        }

        [Fact]
        public async Task GetDocuments_WithNameAndSizeFilter_ReturnsCorrectDocument()
        {
            var collection = _mongoFixture.CreateCollection<TestDocument>();
            var documentA = new TestDocument("A", 1);
            var documentB = new TestDocument("A", 2);
            await collection.InsertOneAsync(documentA);
            await collection.InsertOneAsync(documentB);
            var filter = new JObject()
            {
                {"Name", "A"},
                {
                    "Nested", new JObject()
                    {
                        {"Size", 2}
                    }
                }
            }.ToString();

            var result = await _functionsApi.GetDocuments<TestDocument>(_mongoFixture.GetCollectionName(), filter);

            result.IsSuccessStatusCode.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(documentB);
        }

        [Fact]
        public async Task GetDocuments_WithGtFilter_ReturnsCorrectDocuments()
        {
            var collection = _mongoFixture.CreateCollection<TestDocument>();
            var documentA = new TestDocument("A", 3);
            var documentB = new TestDocument("B", 5);
            var documentC = new TestDocument("C", 10);
            await collection.InsertOneAsync(documentA);
            await collection.InsertOneAsync(documentB);
            await collection.InsertOneAsync(documentC);
            var filter = @"{""Nested.Size"" : {$gt: 4}}";
            
            var result = await _functionsApi.GetDocuments<TestDocument>(_mongoFixture.GetCollectionName(), filter);

            result.IsSuccessStatusCode.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(documentB, documentC);
        }

        public void Dispose()
        {
            _mongoFixture.Dispose();
        }
        
        // ReSharper disable UnusedMember.Local
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable MemberCanBePrivate.Local
        private class TestDocument
        {
            [JsonConverter(typeof(ObjectIdConverter))]
            public ObjectId Id { get; set; }
            
            public string Name { get; set; }

            public NestedTestDocument Nested { get; set; }

            public TestDocument(string name, int size)
            {
                Name = name;
                Nested = new NestedTestDocument(size);
            }
        }

        private class NestedTestDocument
        {
            public int Size { get; set; }

            public NestedTestDocument(int size)
            {
                Size = size;
            }
        }
    }
}