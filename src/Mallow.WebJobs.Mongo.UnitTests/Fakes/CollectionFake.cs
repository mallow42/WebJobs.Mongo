using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Mallow.Azure.WebJobs.Extensions.Mongo.Converters;
using Mallow.WebJobs.Mongo.UnitTests.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mallow.WebJobs.Mongo.UnitTests.Fakes
{
    internal class CollectionFake : IBsonCollection
    {
        private readonly IList<BsonDocument> _documents = new List<BsonDocument>();
        private readonly IDictionary<string, IEnumerable<BsonDocument>> _filteredDocuments = new Dictionary<string, IEnumerable<BsonDocument>>();

        public IEnumerable<BsonDocument> Documents => _documents;

        public Task<List<BsonDocument>> FindAsync(FilterDefinition<BsonDocument> filter, CancellationToken token)
        {
            if (_filteredDocuments.ContainsKey(filter.ToJson()))
            {
                return Task.FromResult(_filteredDocuments[filter.ToJson()].ToList());
            }

            return Task.FromResult(new List<BsonDocument>());
        }

        public Task<BsonDocument> FindOneOrDefaultAsync(FilterDefinition<BsonDocument> filter, CancellationToken token)
        {
            var filterBson = filter.AsBson();
            if (filterBson.Contains(FilterBuilder.ID_FIELD))
            {
                var filterId = filterBson[FilterBuilder.ID_FIELD].ToString();
                var document = Documents.FirstOrDefault(d => d[FilterBuilder.ID_FIELD].ToString() == filterId);
                return Task.FromResult(document);
            }

            return null;
        }

        public Task InsertManyAsync(IEnumerable<BsonDocument> documents, CancellationToken token)
        {
           foreach (var document in documents)
           {
                _documents.Add(EnsureId(document));               
           }
           return Task.CompletedTask;
        }

        public Task UpsertOneAsync(FilterDefinition<BsonDocument> filter, BsonDocument document,
            CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task ReplaceOneAsync(FilterDefinition<BsonDocument> filter, BsonDocument document, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public void AddDocument(object data)
        {
            var bsonDocument = data.ToBsonDocument();
            _documents.Add(EnsureId(bsonDocument));
        }
        
        public void AddDocuments(FilterDefinition<BsonDocument> filter, params object[] data)
        {
            _filteredDocuments.Add(filter.ToJson(), data.Select(d => d.ToBsonDocument()));
        }

        private static BsonDocument EnsureId(BsonDocument bsonDocument)
        {
            if (!bsonDocument.Contains(FilterBuilder.ID_FIELD))
            {
                bsonDocument.Add(FilterBuilder.ID_FIELD, ObjectId.GenerateNewId());
            }

            return bsonDocument;
        }
    }
}