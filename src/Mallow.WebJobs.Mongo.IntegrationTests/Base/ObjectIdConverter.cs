using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal class ObjectIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object  existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return ObjectId.Parse(reader.Value.ToString());
            }
                
            throw new JsonException($"Can convert to ObjectId from string only but found {reader.TokenType}");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObjectId);
        }
    }
}