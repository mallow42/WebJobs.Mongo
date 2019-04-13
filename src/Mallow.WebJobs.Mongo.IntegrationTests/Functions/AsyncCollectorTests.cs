using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.WebJobs.Mongo.IntegrationTests.Base;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Xunit;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Functions
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

        [Fact]
        public async Task InsertOrCreateNewDocuments_NewDocuments_InsertsAllDocuments()
        {
            var collection = _mongoFixture.CreateCollection<TestDocumentWithId>();
            var data = new[]
            {
                new TestDocumentWithId("A", "id-1"), 
                new TestDocumentWithId("B", "id-2")
            };
            
            var result = await _functionsApi.InsertOrCreateNewDocuments(_mongoFixture.GetCollectionName(), data);
            var documents = await collection.Find(FilterDefinition<TestDocumentWithId>.Empty).ToListAsync();

            result.IsSuccessStatusCode.Should().BeTrue();
            documents.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task InsertOrCreateNewDocuments_ExistingDocuments_InsertsAllDocuments()
        {
            var collection = _mongoFixture.CreateCollection<TestDocumentWithId>();
            await collection.InsertOneAsync(new TestDocumentWithId("A", "id-1"));
            await collection.InsertOneAsync(new TestDocumentWithId("B", "id-2"));
            var data = new[]
            {
                new TestDocumentWithId("B", "id-1"),
                new TestDocumentWithId("C", "id-2"), 
            };
            
            var result = await _functionsApi.InsertOrCreateNewDocuments(_mongoFixture.GetCollectionName(), data);
            var documents = await collection.Find(FilterDefinition<TestDocumentWithId>.Empty).ToListAsync();

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
        
        // ReSharper disable UnusedMember.Local
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable MemberCanBePrivate.Local
        [BsonIgnoreExtraElements]
        private class TestDocumentWithId
        {
            public string Name { get; }
            
            public string Id { get; }

            public TestDocumentWithId(string name, string id)
            {
                Name = name;
                Id = id;
            }
        }

        public void Dispose()
        {
            _mongoFixture.Dispose();
        }
    }
}