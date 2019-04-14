using System;
using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo.Converters;
using MongoDB.Bson;
using Xunit;

namespace Mallow.WebJobs.Mongo.UnitTests.Converters
{
    public class UpdateBuilderTests
    {
        [Fact]
        public void CreateUpdate_DocumentWithStringId_ExtractsId()
        {
            var document = new TestDocumentWithId<string>("A", "id-A");
            
            var documentUpdate = UpdateBuilder.CreateUpdate(document);

            documentUpdate.Id.Should().Be("id-A");
            documentUpdate.Update.Should().BeEquivalentTo(new BsonDocument() {{"Name", "A"}});
        }
        
        [Fact]
        public void CreateUpdate_DocumentWithIntId_ExtractsId()
        {
            var document = new TestDocumentWithId<int>("A", 42);
            
            var documentUpdate = UpdateBuilder.CreateUpdate(document);

            documentUpdate.Id.Should().Be(42);
            documentUpdate.Update.Should().BeEquivalentTo(new BsonDocument() {{"Name", "A"}});
        }

        [Fact]
        public void CreateUpdate_DocumentWithObjectId_ExtractsId()
        {
            var id = ObjectId.GenerateNewId();
            var document = new TestDocumentWithId<ObjectId>("A", id);
            
            var documentUpdate = UpdateBuilder.CreateUpdate(document);

            documentUpdate.Id.Should().Be(id);
            documentUpdate.Update.Should().BeEquivalentTo(new BsonDocument() {{"Name", "A"}});
        }
        
        
        [Fact]
        public void  CreateUpdate_DocumentWithoutId_ThrowsException()
        {
            var document = new TestDocument("A");

            Action action = () => UpdateBuilder.CreateUpdate(document);

            action.Should().Throw<InvalidOperationException>();
        }
        
        private class TestDocument
        {
            public string Name { get; }

            public TestDocument(string name)
            {
                Name = name;
            }
        }
        
        private class TestDocumentWithId<T>
        {
            public string Name { get; }
            
            public T Id { get; }

            public TestDocumentWithId(string name, T id)
            {
                Name = name;
                Id = id;
            }
        }
    }
}