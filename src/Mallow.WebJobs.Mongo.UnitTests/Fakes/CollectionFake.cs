using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Mallow.WebJobs.Mongo.UnitTests.Fakes
{
    internal class CollectionFake : IBsonCollection
    {
        private const string ID_FIELD = "_id";
        
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

        public Task<BsonDocument> FindOneOrDefaultAsync(FilterDefinition<BsonDocument> filter,
            CancellationToken token)
        {
            var temp = filter.Render(new BsonDocumentSerializer(), BsonSerializer.SerializerRegistry);
            if (temp.Contains(ID_FIELD))
            {
                var filterId = temp[ID_FIELD].ToString();
                var document = Documents.FirstOrDefault(d => d[ID_FIELD].ToString() == filterId);
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
            if (!bsonDocument.Contains(ID_FIELD))
            {
                bsonDocument.Add(ID_FIELD, ObjectId.GenerateNewId());
            }

            return bsonDocument;
        }
    }
}