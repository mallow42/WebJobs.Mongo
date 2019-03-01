using System.Collections.Generic;
using System.Threading.Tasks;
using Mallow.Azure.WebJobs.Extensions.Mongo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using MongoDB.Bson;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Functions.Functions
{
    public class GetDocuments
    {
        [FunctionName(nameof(GetDocuments))]
        public static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest  req,
            [Mongo(DatabaseId = "%MongoDatabaseId%", CollectionId = "{Query.collection}", ConnectionString = "%MongoConnectionString%", Filter = "{Query.filter}")] IEnumerable<TestDocument> input)
        {
            return Task.FromResult<IActionResult>(new OkObjectResult(input));
        }
        
        public class TestDocument
        {
            public ObjectId Id { get; set; }
            
            public string Name { get; set; }

            public NestedTestDocument Nested { get; set; }
        }
        
        public class NestedTestDocument
        {
            public int Size { get; set; }
        }
    }
}