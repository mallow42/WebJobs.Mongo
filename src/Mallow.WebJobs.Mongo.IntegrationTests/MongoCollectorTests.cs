using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo;
using Mallow.Azure.WebJobs.Extensions.Mongo.Collector;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Mallow.WebJobs.Mongo.IntegrationTests.Base;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Xunit;

namespace Mallow.WebJobs.Mongo.IntegrationTests
{
    public class MongoCollectorTests : IDisposable
    { 
        private readonly MongoFixture _mongoFixture;
        private readonly MongoCollectorBuilder<TestDocumentWithId> _builder;

        public MongoCollectorTests()
        {
            _mongoFixture = new MongoFixture();
            _builder = new MongoCollectorBuilder<TestDocumentWithId>(new MongoCollectionFactory());
        }

        [Theory]
        [InlineData(InsertMode.Create)]
        [InlineData(InsertMode.CreateOrReplace)]
        public async Task CreateOrReplace_UniqueDocuments_AdsNewDocuments(InsertMode mode)
        {
            var collection = _mongoFixture.CreateCollection<TestDocumentWithId>();
            var attribute = _mongoFixture.CreateAttribute(mode);
            var collector = _builder.Convert(attribute);
            var documentA = new TestDocumentWithId("A", "id-A");
            var documentB = new TestDocumentWithId("B", "id-B");
            
            await collector.AddAsync(documentA);
            await collector.AddAsync(documentB);
            await collector.FlushAsync();
            
            var documents = await collection.GetAllDocuments();
            documents.Should().BeEquivalentTo(documentA, documentB);
        }

        [Fact]
        public async Task Create_DuplicateIds_ThrowsException()
        {
            var collection = _mongoFixture.CreateCollection<TestDocumentWithId>();
            var attribute = _mongoFixture.CreateAttribute(InsertMode.Create);
            var collector = _builder.Convert(attribute);
            var documentA = new TestDocumentWithId("A", "id-A");
            var documentB = new TestDocumentWithId("B", "id-B");
            await documentA.InsertInto(collection);
            await documentB.InsertInto(collection);
            
            await collector.AddAsync(documentA.ChangeNameTo("C"));
            await collector.AddAsync(documentB.ChangeNameTo("D"));
            Func<Task> action = () => collector.FlushAsync();

            action.Should().Throw<MongoException>();
            var documents = await collection.GetAllDocuments();
            documents.Should().BeEquivalentTo(documentA, documentB);
        }

        [Theory]
        [InlineData(InsertMode.Replace)]
        [InlineData(InsertMode.CreateOrReplace)]
        public async Task CreateOrReplace_ExistingDocument_ReplacesDocuments(InsertMode mode)
        {
            var collection = _mongoFixture.CreateCollection<TestDocumentWithId>();
            var attribute = _mongoFixture.CreateAttribute(mode);
            var collector = _builder.Convert(attribute);
            var documentA = new TestDocumentWithId("A", "id-A");
            var documentB = new TestDocumentWithId("B", "id-B");
            var updatedDocumentA = documentA.ChangeNameTo("C");
            var updatedDocumentB = documentB.ChangeNameTo("D");
            await documentA.InsertInto(collection);
            await documentB.InsertInto(collection);

            await collector.AddAsync(updatedDocumentA);
            await collector.AddAsync(updatedDocumentB);
            await collector.FlushAsync();

            var documents = await collection.GetAllDocuments();
            documents.Should().BeEquivalentTo(updatedDocumentA, updatedDocumentB);
        }

        [Fact]
        public async Task Replace_NonExistingDocument_DoesNothing()
        {
            var collection = _mongoFixture.CreateCollection<TestDocumentWithId>();
            var attribute = _mongoFixture.CreateAttribute(InsertMode.Replace);
            var collector = _builder.Convert(attribute);
            var documentA = new TestDocumentWithId("A", "id-A");
            
            await collector.AddAsync(documentA);
            await collector.FlushAsync();

            var documents = await collection.GetAllDocuments();
            documents.Should().BeEmpty();
        }

        public void Dispose()
        {
            _mongoFixture.Dispose();
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

            public TestDocumentWithId ChangeNameTo(string name)
            {
                return new TestDocumentWithId(name, Id);
            }

            public Task InsertInto(IMongoCollection<TestDocumentWithId> collection)
            {
                return collection.InsertOneAsync(this);
            }
        }
    }
}