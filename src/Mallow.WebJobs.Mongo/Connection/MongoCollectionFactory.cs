using System.Collections.Concurrent;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Connection
{
    internal class MongoCollectionFactory : IMongoCollectionFactory
    {
        private readonly ConcurrentDictionary<string, MongoClient> _clientCache = new ConcurrentDictionary<string, MongoClient>();
        private readonly ConcurrentDictionary<string, IMongoDatabase> _databaseCache = new ConcurrentDictionary<string, IMongoDatabase>();
        private readonly ConcurrentDictionary<string, IBsonCollection> _collectionCache = new ConcurrentDictionary<string, IBsonCollection>();

        public IBsonCollection Create(ConnectionSettings connectionSettings)
        {
             var client = _clientCache.GetOrAdd(ClientCacheKey(connectionSettings), _ => new MongoClient(connectionSettings.ConnectionString));
             var database = _databaseCache.GetOrAdd(DatabaseCacheKey(connectionSettings), _ => client.GetDatabase(connectionSettings.DatabaseId));
             return _collectionCache.GetOrAdd(CollectionCacheKey(connectionSettings), _ => new BsonCollectionAdapter(database.GetCollection<BsonDocument>(connectionSettings.CollectionId)));
        }

        private string ClientCacheKey(ConnectionSettings connectionSettings)
        {
            return connectionSettings.ConnectionString;
        }
        
        private string DatabaseCacheKey(ConnectionSettings connectionSettings)
        {
            return $"{connectionSettings.ConnectionString}|{connectionSettings.DatabaseId}";
        }
        
        private string CollectionCacheKey(ConnectionSettings connectionSettings)
        {
            return $"{connectionSettings.ConnectionString}|{connectionSettings.DatabaseId}|{connectionSettings.CollectionId}";
        }
    }
}