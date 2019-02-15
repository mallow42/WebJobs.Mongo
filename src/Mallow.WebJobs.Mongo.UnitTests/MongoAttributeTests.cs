using FluentAssertions;
using Mallow.Azure.WebJobs.Extensions.Mongo;
using Xunit;

namespace Mallow.WebJobs.Mongo.UnitTests
{
    public class MongoAttributeTests
    {
        [Fact]
        public void ToConnectionSettings_CreatesConnectionSettings()
        {
            var attribute = new MongoAttribute()
            {
                DatabaseId = "DatabaseA",
                CollectionId = "CollectionA",
                ConnectionString = "mongodb://localhost:27017"
            };

            var settings = attribute.ToConnectionSettings();

            settings.DatabaseId.Should().Be("DatabaseA");
            settings.CollectionId.Should().Be("CollectionA");
            settings.ConnectionString.Should().Be("mongodb://localhost:27017");
        }
    }
}