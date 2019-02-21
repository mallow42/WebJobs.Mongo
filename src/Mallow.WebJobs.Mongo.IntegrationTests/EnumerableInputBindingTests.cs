using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.WebJobs.Mongo.IntegrationTests.Base;
using MongoDB.Bson;
using Newtonsoft.Json;
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
            var documentA = new TestDocument()
            {
                Name = "A"
            };
            var documentB = new TestDocument()
            {
                Name = "B"
            };
            await collection.InsertOneAsync(documentA);
            await collection.InsertOneAsync(documentB);
            
            var result = await _functionsApi.GetDocuments<TestDocument>(_mongoFixture.GetCollectionName(), "{\"Name\" : \"A\"}");

            result.IsSuccessStatusCode.Should().BeTrue();
            result.Content.Should().BeEquivalentTo(documentA);
        }
        
        public void Dispose()
        {
            _mongoFixture.Dispose();
        }
        
        private class TestDocument
        {
            [JsonConverter(typeof(ObjectIdConverter))]
            public ObjectId Id { get; set; }
            
            public string Name { get; set; }
        }
    }
}