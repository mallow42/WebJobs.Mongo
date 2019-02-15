using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal class FunctionsApi
    {
        private const string BASE_URL = "http://localhost:7071/api";

        public async Task<RequestResult> GetDocumentByObjectId(string collectionName, ObjectId documentId)
        {
            var response = await BASE_URL.AppendPathSegment("GetDocumentByObjectId")
                .SetQueryParam("collection", collectionName)
                .SetQueryParam("id", documentId.ToString())
                .AllowAnyHttpStatus()
                .GetAsync();

            return await CreateRequestResult(response);
        }

        public async Task<RequestResult> GetDocumentByStringId(string collectionName, string documentId)
        {
            var response = await BASE_URL.AppendPathSegment("GetDocumentByStringId")
                .SetQueryParam("collection", collectionName)
                .SetQueryParam("id", documentId)
                .AllowAnyHttpStatus()
                .GetAsync();
            
            return await CreateRequestResult(response);
        }

        public async Task<RequestResult> InsertNewDocuments(string collectionName, object data)
        {
            var response = await BASE_URL.AppendPathSegment("InsertNewDocuments")
                .SetQueryParam("collection", collectionName)
                .AllowAnyHttpStatus()
                .PostJsonAsync(data);
            
            return await CreateRequestResult(response);
        }
        
        private static async Task<RequestResult> CreateRequestResult(HttpResponseMessage result)
        {
            var content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return new RequestResult(JObject.Parse(content), result.StatusCode);
            }

            return new RequestResult(content, result.StatusCode);
        }
    }
}