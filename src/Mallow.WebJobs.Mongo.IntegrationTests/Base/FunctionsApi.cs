using System.Threading.Tasks;
using MongoDB.Bson;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal class FunctionsApi
    {
        private const string BASE_URL = "http://localhost:7071/api";

        public Task<RequestResult> GetDocumentByObjectId(string collectionName, ObjectId documentId)
        {
            return Client.GetAsync($"{BASE_URL}/GetDocumentByObjectId?collection={collectionName}&id={documentId.ToString()}");
        }

        public Task<RequestResult> GetDocumentByStringId(string collectionName, string documentId)
        {
            return Client.GetAsync($"{BASE_URL}/GetDocumentByStringId?collection={collectionName}&id={documentId}");
        }

        public Task<RequestResult> InsertNewDocuments(string collectionName, object data)
        {
            return Client.PostAsync($"{BASE_URL}/InsertNewDocuments?collection={collectionName}", data);
        }
    }
}