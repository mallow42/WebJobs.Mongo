using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Converters
{
    internal static class FilterBuilder
    {
        public const string ID_FIELD = "_id";

        public static FilterDefinition<BsonDocument> CreateFilterByDocumentId(BsonDocument document)
        {
            if (document.Contains(ID_FIELD))
            {
                return CreateFilter(BsonTypeMapper.MapToDotNetValue(document[ID_FIELD]));
            }
            
            throw new InvalidOperationException("Document must contain id property");
        }
        
        public static FilterDefinition<BsonDocument> CreateFilter(string id)
        {
            return CreateFilter(GetId(id));
        }

        private static FilterDefinition<BsonDocument> CreateFilter(object id)
        {
            return Builders<BsonDocument>.Filter.Eq(ID_FIELD, id);
        }

        private static object GetId(string id)
        {
            if(ObjectId.TryParse(id, out var objectId))
            {
                return objectId;
            }

            return id;
        }
    }
}