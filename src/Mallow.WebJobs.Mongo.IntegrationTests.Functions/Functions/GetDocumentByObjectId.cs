using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo;
using Mallow.WebJobs.Mongo.IntegrationTests.Functions.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Functions.Functions
{
    public static class GetDocumentByObjectId
    {
        [FunctionName(nameof(GetDocumentByObjectId))]
        public static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Mongo(DatabaseId = "%MongoDatabaseId%", CollectionId = "{Query.collection}", ConnectionString = "%MongoConnectionString%", Id = "{Query.id}")] TestDocumentWithObjectId input)
        {
            if (input != null)
            {
                return Task.FromResult<IActionResult>(new OkObjectResult(input));
            }
            
            return Task.FromResult<IActionResult>(new BadRequestObjectResult("Document not found"));
        }
    }
}
