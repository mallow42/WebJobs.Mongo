using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Mallow.WebJobs.Mongo.UnitTests.Base
{
    [BsonIgnoreExtraElements]
    internal class TestDocument
    {
        public string Name { get; }

        public TestDocument(string name)
        {
            Name = name;
        }
    }
}