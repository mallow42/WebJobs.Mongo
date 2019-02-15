# Azure Functions MongoDB Extensions

This repo contains [MongoDB](https://www.mongodb.com/) binding extensions for the **Azure WebJobs SDK**. See the [Azure WebJobs SDK repo](https://github.com/Azure/azure-webjobs-sdk) for more information. The binding extensions in this repo are available as the **Mallow.WebJobs.Mongo** nuget package.

## Usage

### Input binding

Example azure function that can be used to obtain one document via it's ID.

```csharp
public static Task<IActionResult> GetDocumentById
(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
    [Mongo(DatabaseId = "TestDb", CollectionId = "Collection", ConnectionString = "%ConnectionString%", Id = "{Query.id}")] Document input
)
{
    if (input != null)
    {
        return Task.FromResult<IActionResult>(new OkObjectResult(input));
    }

    return Task.FromResult<IActionResult>(new BadRequestObjectResult("Document not found"));
}
```

### Output binding

Example azure function that can be used to create multiple new documents.

```csharp
public static async Task<IActionResult> InsertNewDocuments
(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage  req,
    [Mongo(DatabaseId = "TestDb", CollectionId = "Collection", ConnectionString = "%MongoConnectionString%")] IAsyncCollector<AsyncCollectorTestDocument> collector
)
{
    var documents = await req.Content.ReadAsAsync<Document[]>();
    foreach (var document in documents)
    {
        await collector.AddAsync(document);
    }

    return new OkObjectResult(new {Status = "OK"});
}
```

## License

[the MIT License](LICENSE)