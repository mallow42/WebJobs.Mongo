using System.Net;
using Newtonsoft.Json.Linq;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal class RequestResult
    {
        public string ErrorMessage { get; }
        
        public HttpStatusCode ResultStatusCode { get; }
        
        public bool IsSuccessStatusCode { get; }
        
        public JObject Content { get; }

        public RequestResult(JObject content, HttpStatusCode resultStatusCode)
        {
            IsSuccessStatusCode = true;
            Content = content;
            ResultStatusCode = resultStatusCode;
        }

        public RequestResult(string errorMessage, HttpStatusCode resultStatusCode)
        {
            ErrorMessage = errorMessage;
            ResultStatusCode = resultStatusCode;
        }
    }
}