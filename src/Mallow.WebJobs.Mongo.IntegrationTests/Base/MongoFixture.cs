using System;
using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal class MongoFixture : IDisposable
    {
        private const string CONNECTION_STRING = "mongodb://localhost:27017";
        private const string DATABASE_ID = "IntegrationTestsDB";
        
        private readonly IMongoDatabase _db;

        private  string _collectionName;

        public MongoFixture()
        {
            var mongo = new MongoClient(CONNECTION_STRING);
            _db = mongo.GetDatabase(DATABASE_ID);
        }
        
        public IMongoCollection<BsonDocument> CreateCollection([CallerMemberName] string testName = "")
        {
            return CreateCollection<BsonDocument>(testName);
        }
        
        public IMongoCollection<T> CreateCollection<T>([CallerMemberName] string testName = "")
        {
            _collectionName = CreateCollectionName(testName);
            return _db.GetCollection<T>(_collectionName);
        }

        private static string CreateCollectionName(string testName)
        {
            return $"{testName}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        }

        public string GetCollectionName([CallerMemberName] string testName = "")
        {
            return _collectionName ?? CreateCollectionName(testName);
        }

        public void Dispose()
        {
            if (_collectionName != null)
            {
                _db.DropCollection(_collectionName);
            }
        }
    }
}