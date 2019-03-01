using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Connection
{
    internal class BsonCollectionAdapter : IBsonCollection
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public BsonCollectionAdapter(IMongoCollection<BsonDocument> collection)
        {
            _collection = collection;
        }
        
        public Task<List<BsonDocument>> FindAsync(FilterDefinition<BsonDocument> filter, CancellationToken token)
        {
            return _collection.Find(filter).ToListAsync(token);
        }

        public Task<BsonDocument> FindOneOrDefaultAsync(FilterDefinition<BsonDocument> filter, CancellationToken token)
        {
            return _collection.Find(filter).FirstOrDefaultAsync(token);
        }

        public Task InsertManyAsync(IEnumerable<BsonDocument> documents, CancellationToken token)
        {
            return _collection.InsertManyAsync(documents, null, token);
        }
    }
}