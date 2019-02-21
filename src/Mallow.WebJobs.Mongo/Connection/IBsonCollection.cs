using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Connection
{
    internal interface IBsonCollection
    {
        Task<List<BsonDocument>> FindAsync(FilterDefinition<BsonDocument> filter, CancellationToken token);
        
        Task<BsonDocument> FindOneOrDefaultAsync(FilterDefinition<BsonDocument> filter, CancellationToken token);

        Task InsertManyAsync(IEnumerable<BsonDocument> documents, CancellationToken token);
    }
}