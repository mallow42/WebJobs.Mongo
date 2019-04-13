using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal class FunctionsApi
    {
        private const string BASE_URL = "http://localhost:8080/api";

        public Task<RequestResult<JObject>> GetDocumentByObjectId(string collectionName, ObjectId documentId)
        {
            return BASE_URL.AppendPathSegment("GetDocumentByObjectId")
                           .SetQueryParam("collection", collectionName)
                           .SetQueryParam("id", documentId.ToString())
                           .AllowAnyHttpStatus()
                           .GetAsync()
                           .CreateRequestResult<JObject>();
        }

        public Task<RequestResult<JObject>> GetDocumentByStringId(string collectionName, string documentId)
        {
            return BASE_URL.AppendPathSegment("GetDocumentByStringId")
                           .SetQueryParam("collection", collectionName)
                           .SetQueryParam("id", documentId)
                           .AllowAnyHttpStatus()
                           .GetAsync()
                           .CreateRequestResult<JObject>();
        }

        public Task<RequestResult<JObject>> InsertNewDocuments(string collectionName, object data)
        {
            return BASE_URL.AppendPathSegment("InsertNewDocuments")
                           .SetQueryParam("collection", collectionName)
                           .AllowAnyHttpStatus()
                           .PostJsonAsync(data)
                           .CreateRequestResult<JObject>();
        }

        public Task<RequestResult<JObject>> InsertOrCreateNewDocuments(string collectionName, object data)
        {
            return BASE_URL.AppendPathSegment("InsertOrCreateNewDocuments")
                           .SetQueryParam("collection", collectionName)
                           .AllowAnyHttpStatus()
                           .PostJsonAsync(data)
                           .CreateRequestResult<JObject>();
        }

        public Task<RequestResult<IEnumerable<T>>> GetDocuments<T>(string collectionName, string filter)
        {
            return BASE_URL.AppendPathSegment("GetDocuments")
                           .SetQueryParam("collection", collectionName)
                           .SetQueryParam("filter", filter)
                           .AllowAnyHttpStatus()
                           .GetAsync()
                           .CreateRequestResult<IEnumerable<T>>();
        }
    }
}