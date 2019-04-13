using System.Net.Http;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo;
using Mallow.WebJobs.Mongo.IntegrationTests.Functions.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Functions.Functions
{
    public class InsertOrCreateNewDocuments
    {
        [FunctionName(nameof(InsertOrCreateNewDocuments))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage  req,
            [Mongo(DatabaseId = "%MongoDatabaseId%", 
                   CollectionId = "{Query.collection}", 
                   ConnectionString = "%MongoConnectionString%", 
                   Mode = InsertMode.CreateOrReplace)] IAsyncCollector<AsyncCollectorTestDocumentWithId> collector)
        {
            var documents = await req.Content.ReadAsAsync<AsyncCollectorTestDocumentWithId[]>();
            foreach (var document in documents)
            {
                await collector.AddAsync(document);
            }

            return new OkObjectResult(new {Status = "OK"});
        }
    }
}