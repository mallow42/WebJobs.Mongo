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