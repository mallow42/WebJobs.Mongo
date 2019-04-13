using System.Collections.Generic;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo;
using MongoDB.Driver;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal static class MongoTestExtensions
    {
        public static MongoAttribute CreateAttribute(this MongoFixture mongoFixture, InsertMode insertMode)
        {
            return new MongoAttribute()
            {
                CollectionId = mongoFixture.GetCollectionName(),
                DatabaseId = mongoFixture.GetDatabaseId(),
                ConnectionString = mongoFixture.GetConnectionString(),
                Mode = insertMode
            };
        }
        
        public static Task<List<T>> GetAllDocuments<T>(this IMongoCollection<T> collection)
        {
            return collection.Find(FilterDefinition<T>.Empty).ToListAsync();
        }
    }
}