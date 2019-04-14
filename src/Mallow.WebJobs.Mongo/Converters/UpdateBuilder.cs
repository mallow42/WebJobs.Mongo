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
                var id = BsonTypeMapper.MapToDotNetValue(document[FilterBuilder.ID_FIELD]);
                document.Remove(FilterBuilder.ID_FIELD);
                return new DocumentUpdate(document, id);
            }
            
            throw new InvalidOperationException("Document must contain id property");
        }
    }
}