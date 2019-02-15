using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MongoDB.Bson;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal class FunctionsApi
    {
        private const string BASE_URL = "http://localhost:7071/api";

        public Task<RequestResult> GetDocumentByObjectId(string collectionName, ObjectId documentId)
        {
            return BASE_URL.AppendPathSegment("GetDocumentByObjectId")
                           .SetQueryParam("collection", collectionName)
                           .SetQueryParam("id", documentId.ToString())
                           .AllowAnyHttpStatus()
                           .GetAsync()
                           .CreateRequestResult();
        }

        public Task<RequestResult> GetDocumentByStringId(string collectionName, string documentId)
        {
            return BASE_URL.AppendPathSegment("GetDocumentByStringId")
                           .SetQueryParam("collection", collectionName)
                           .SetQueryParam("id", documentId)
                           .AllowAnyHttpStatus()
                           .GetAsync()
                           .CreateRequestResult();
        }

        public Task<RequestResult> InsertNewDocuments(string collectionName, object data)
        {
            return BASE_URL.AppendPathSegment("InsertNewDocuments")
                           .SetQueryParam("collection", collectionName)
                           .AllowAnyHttpStatus()
                           .PostJsonAsync(data)
                           .CreateRequestResult();
        }
    }
}