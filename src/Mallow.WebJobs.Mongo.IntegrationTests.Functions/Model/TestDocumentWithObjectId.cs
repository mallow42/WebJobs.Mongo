using MongoDB.Bson;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Functions.Model
{
    public class TestDocumentWithObjectId
    {
        public string Name { set; get; }

        public ObjectId Id { get; set; }
    }
}