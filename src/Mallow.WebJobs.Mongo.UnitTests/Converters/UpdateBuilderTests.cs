using System;
using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo.Converters;
using Mallow.WebJobs.Mongo.UnitTests.Base;
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

            documentUpdate.Update.Should().BeEquivalentTo(new BsonDocument() {{"Name", "A"}});
        }
        
        [Fact]
        public void CreateUpdate_DocumentWithIntId_ExtractsId()
        {
            var document = new TestDocumentWithId<int>("A", 42);
            
            var documentUpdate = UpdateBuilder.CreateUpdate(document);

            documentUpdate.Update.Should().BeEquivalentTo(new BsonDocument() {{"Name", "A"}});
        }

        [Fact]
        public void CreateUpdate_DocumentWithObjectId_ExtractsId()
        {
            var id = ObjectId.GenerateNewId();
            var document = new TestDocumentWithId<ObjectId>("A", id);
            
            var documentUpdate = UpdateBuilder.CreateUpdate(document);

            documentUpdate.Update.Should().BeEquivalentTo(new BsonDocument() {{"Name", "A"}});
        }

        [Fact]
        public void CreateUpdate_DocumentWithStringId_CreatesCorrectFilter()
        {
            var document = new TestDocumentWithId<string>("A", "id-A");
            
            var documentUpdate = UpdateBuilder.CreateUpdate(document);

            var filter = documentUpdate.Filter.AsBson();
            filter.Contains(FilterBuilder.ID_FIELD).Should().BeTrue();
            filter[FilterBuilder.ID_FIELD].IsString.Should().BeTrue();
            filter[FilterBuilder.ID_FIELD].AsString.Should().Be("id-A");
        }
        
        [Fact]
        public void CreateUpdate_DocumentWithIntId_CreatesCorrectFilter()
        {
            var document = new TestDocumentWithId<int>("A", 42);
            
            var documentUpdate = UpdateBuilder.CreateUpdate(document);

            var filter = documentUpdate.Filter.AsBson();
            filter.Contains(FilterBuilder.ID_FIELD).Should().BeTrue();
            filter[FilterBuilder.ID_FIELD].IsInt32.Should().BeTrue();
            filter[FilterBuilder.ID_FIELD].AsInt32.Should().Be(42);
        }

        [Fact]
        public void CreateUpdate_DocumentWithObjectId_CreatesCorrectFilter()
        {
            var id = ObjectId.GenerateNewId();
            var document = new TestDocumentWithId<ObjectId>("A", id);
            
            var documentUpdate = UpdateBuilder.CreateUpdate(document);

            var filter = documentUpdate.Filter.AsBson();
            filter.Contains(FilterBuilder.ID_FIELD).Should().BeTrue();
            filter[FilterBuilder.ID_FIELD].IsObjectId.Should().BeTrue();
            filter[FilterBuilder.ID_FIELD].AsObjectId.Should().Be(id);
        }
        
        
        [Fact]
        public void  CreateUpdate_DocumentWithoutId_ThrowsException()
        {
            var document = new TestDocument("A");

            Action action = () => UpdateBuilder.CreateUpdate(document);

            action.Should().Throw<InvalidOperationException>();
        }
        
        
    }
}