using Mallow.Azure.WebJobs.Extensions.Mongo;

namespace Mallow.WebJobs.Mongo.UnitTests.Base
{
    internal static class MongoAttributeFactory
    {
        public static MongoAttribute CreateWithFilter(string filter)
        {
            var attribute = CreateMongoAttribute();
            attribute.Filter = filter;
            return attribute;
        }
        
        public static MongoAttribute CreateWithId(string id)
        {
            var attribute = CreateMongoAttribute();
            attribute.Id = id;
            return attribute;
        }
        
        private static MongoAttribute CreateMongoAttribute()
        {
            return new MongoAttribute()
            {
                DatabaseId = "DatabaseA",
                CollectionId = "CollectionA",
                ConnectionString = "mongodb://localhost:27017",
            };
        }
    }
}