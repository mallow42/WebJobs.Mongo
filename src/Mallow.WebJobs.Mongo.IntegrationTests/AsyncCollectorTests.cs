using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.WebJobs.Mongo.IntegrationTests.Base;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Xunit;

namespace Mallow.WebJobs.Mongo.IntegrationTests
{
    public class AsyncCollectorTests : IDisposable
    {
        private readonly FunctionsApi _functionsApi;
        private readonly MongoFixture _mongoFixture;

        public AsyncCollectorTests()
        {
            _mongoFixture = new MongoFixture();
            _functionsApi = new FunctionsApi();
        }

        [Fact]
        public async Task InsertNewDocuments_InsertsAllDocuments()
        {
            var collection = _mongoFixture.CreateCollection<TestDocument>();
            var data = new[]
            {
                new TestDocument("A"),
                new TestDocument("B")
            };
            
            var result = await _functionsApi.InsertNewDocuments(_mongoFixture.GetCollectionName(), data);
            var documents = await collection.Find(FilterDefinition<TestDocument>.Empty).ToListAsync();

            result.IsSuccessStatusCode.Should().BeTrue();
            documents.Should().BeEquivalentTo(data);
        }
        
        // ReSharper disable UnusedMember.Local
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable MemberCanBePrivate.Local
        [BsonIgnoreExtraElements]
        private class TestDocument
        {
            public string Name { get; }

            public TestDocument(string name)
            {
                Name = name;
            }
        }

        public void Dispose()
        {
            _mongoFixture.Dispose();
        }
    }
}