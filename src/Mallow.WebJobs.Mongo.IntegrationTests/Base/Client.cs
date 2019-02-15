using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Base
{
    internal static class Client
    {
        private static readonly HttpClient _client = new HttpClient();
        
        public static async Task<RequestResult> GetAsync(string uri)
        {
            var result = await _client.GetAsync(uri);
            return await CreateRequestResult(result);
        }

        public static async Task<RequestResult> PostAsync(string uri, object data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var result = await _client.PostAsync(uri, new StringContent(jsonData, Encoding.UTF8, "application/json"));
            return await CreateRequestResult(result);
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