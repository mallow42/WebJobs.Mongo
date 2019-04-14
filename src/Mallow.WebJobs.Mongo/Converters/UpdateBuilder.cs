using System;
using MongoDB.Bson;

namespace Mallow.Azure.WebJobs.Extensions.Mongo.Converters
{
    internal static class UpdateBuilder
    {
        public static DocumentUpdate CreateUpdate<T>(T item)
        {
            var document = item.ToBsonDocument();
            if (document.Contains(FilterBuilder.ID_FIELD))
            {
                return CreateUpdate(document);
            }
            
            throw new InvalidOperationException("Document must contain id property");
        }

        private static DocumentUpdate CreateUpdate(BsonDocument document)
        {
            var id = BsonTypeMapper.MapToDotNetValue(document[FilterBuilder.ID_FIELD]);
            document.Remove(FilterBuilder.ID_FIELD);
            var filter = FilterBuilder.CreateFilter(id);
            
            return new DocumentUpdate(document, filter);
        }
    }
}