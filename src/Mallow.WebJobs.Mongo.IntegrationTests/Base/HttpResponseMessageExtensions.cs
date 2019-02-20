using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal static class HttpResponseMessageExtensions
    {
        public static async Task<RequestResult> CreateRequestResult(this Task<HttpResponseMessage> resultTask)
        {
            var result = await resultTask;
            var content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return new RequestResult(JObject.Parse(content), result.StatusCode);
            }

            return new RequestResult(content, result.StatusCode);
        }
    }
}