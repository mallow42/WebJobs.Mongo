using System.Net;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal class RequestResult<T>
    {
        public string ErrorMessage { get; }
        
        public HttpStatusCode ResultStatusCode { get; }
        
        public bool IsSuccessStatusCode { get; }
        
        public T Content { get; }

        public RequestResult(T content, HttpStatusCode resultStatusCode)
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