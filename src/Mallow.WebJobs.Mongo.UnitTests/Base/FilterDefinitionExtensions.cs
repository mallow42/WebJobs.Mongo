using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Mallow.WebJobs.Mongo.UnitTests.Base
{
    public static class FilterDefinitionExtensions
    {
        public static BsonDocument AsBson(this FilterDefinition<BsonDocument> filter)
        {
            return filter.Render(new BsonDocumentSerializer(), BsonSerializer.SerializerRegistry);
        }
    }
}