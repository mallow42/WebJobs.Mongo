using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal static class HttpResponseMessageExtensions
    {
        public static async Task<RequestResult<T>> CreateRequestResult<T>(this Task<HttpResponseMessage> resultTask)
        {
            var result = await resultTask;
            var content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                return new RequestResult<T>(JsonConvert.DeserializeObject<T>(content), result.StatusCode);
            }

            return new RequestResult<T>(content, result.StatusCode);
        }
    }
}