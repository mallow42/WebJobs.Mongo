using System;
using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo.Converters;
using Mallow.WebJobs.Mongo.UnitTests.Base;
using MongoDB.Bson;
using Xunit;

namespace Mallow.WebJobs.Mongo.UnitTests.Converters
{
    public class FilterBuilderTests
    {
        [Fact]
        public void CreateFilterByDocumentId_StringId_CreatesFilterWithStringId()
        {
            var document = new BsonDocument(){
            {
                FilterBuilder.ID_FIELD, "id-1"
            }};
            
            var filterBson = FilterBuilder.CreateFilterByDocumentId(document).AsBson();

            filterBson[FilterBuilder.ID_FIELD].IsString.Should().BeTrue();
            filterBson[FilterBuilder.ID_FIELD].AsString.Should().Be("id-1");
        }

        [Fact]
        public void CreateFilterByDocumentId_ObjectIdId_CreatesFilterWithObjectId()
        {
            var id = ObjectId.GenerateNewId();
            var document = new BsonDocument(){
            {
                FilterBuilder.ID_FIELD, id
            }};
            
            var filterBson = FilterBuilder.CreateFilterByDocumentId(document).AsBson();

            filterBson[FilterBuilder.ID_FIELD].IsObjectId.Should().BeTrue();
            filterBson[FilterBuilder.ID_FIELD].AsObjectId.Should().Be(id);
        }

        [Fact]
        public void CreateFilterByDocumentId_IntId_CreatesFilterWithIntId()
        {
            var document = new BsonDocument(){
            {
                FilterBuilder.ID_FIELD, 42
            }};
            
            var filterBson = FilterBuilder.CreateFilterByDocumentId(document).AsBson();

            filterBson[FilterBuilder.ID_FIELD].IsInt32.Should().BeTrue();
            filterBson[FilterBuilder.ID_FIELD].AsInt32.Should().Be(42);
        }

        [Fact]
        public void CreateFilterByDocumentId_DocumentWithoutId_ThrowsException()
        {
            var document = new BsonDocument(){
            {
                "Name", "name"
            }};
            
            Action action = () => FilterBuilder.CreateFilterByDocumentId(document);

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void CreateFilter_WithStringId_CreatesFilterWithStringId()
        {
            var filterBson = FilterBuilder.CreateFilter("id-1").AsBson();
            
            filterBson[FilterBuilder.ID_FIELD].IsString.Should().BeTrue();
            filterBson[FilterBuilder.ID_FIELD].AsString.Should().Be("id-1");
        }
        
        [Fact]
        public void CreateFilter_WithObjectId_CreatesFilterWithObjectId()
        {
            var id = ObjectId.GenerateNewId();
            
            var filterBson = FilterBuilder.CreateFilter(id.ToString()).AsBson();
            
            filterBson[FilterBuilder.ID_FIELD].IsObjectId.Should().BeTrue();
            filterBson[FilterBuilder.ID_FIELD].AsObjectId.Should().Be(id);
        }
    }
}