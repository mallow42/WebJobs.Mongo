using MongoDB.Bson.Serialization.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Mallow.WebJobs.Mongo.UnitTests.Converters
{
    [BsonIgnoreExtraElements]
    internal class TestDocumentWithId<T>
    {
        public string Name { get; }
            
        // ReSharper disable once MemberCanBePrivate.Global
        public T Id { get; }

        public TestDocumentWithId(string name, T id)
        {
            Name = name;
            Id = id;
        }
    }
}