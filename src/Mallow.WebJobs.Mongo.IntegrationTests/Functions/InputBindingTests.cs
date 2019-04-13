using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Mallow.WebJobs.Mongo.IntegrationTests.Base;
using MongoDB.Bson;
using Xunit;

namespace Mallow.WebJobs.Mongo.IntegrationTests.Functions
{
    public class InputBindingTests : IDisposable
    {
        private readonly FunctionsApi _functionsApi;
        private readonly MongoFixture _mongoFixture;

        public InputBindingTests()
        {
            _mongoFixture = new MongoFixture();
            _functionsApi = new FunctionsApi();
        }

        [Fact]
        public async Task GetDocumentByObjectId_WithExistingId_ReturnsCorrectDocument()
        {
            var collection = _mongoFixture.CreateCollection();
            var id = ObjectId.GenerateNewId();
            var document = new BsonDocument()
            {
                {"_id", id},
                {"Name", "Mr. Awesome"}
            };
            await collection.InsertOneAsync(document);
            
            var result = await _functionsApi.GetDocumentByObjectId(_mongoFixture.GetCollectionName(), id);

            result.IsSuccessStatusCode.Should().BeTrue();
            result.Content.Value<string>("name").Should().Be("Mr. Awesome");
            result.Content.Value<string>("id").Should().Be(id.ToString());
        }

        [Fact]
        public async Task GetDocumentByObjectId_WithNonExistingId_ReturnsBadRequest()
        {
            var result = await _functionsApi.GetDocumentByObjectId(_mongoFixture.GetCollectionName(), ObjectId.GenerateNewId());

            result.ResultStatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.ErrorMessage.Should().Be("Document not found");
        }
        
        [Fact]
        public async Task GetDocumentByStringId_WithExistingId_ReturnsCorrectDocument()
        {
            var collection = _mongoFixture.CreateCollection();
            var id = Guid.NewGuid().ToString();
            var document = new BsonDocument()
            {
                {"_id", id},
                {"Name", "Mr. Awesome"}
            };
            await collection.InsertOneAsync(document);

            var result = await _functionsApi.GetDocumentByStringId(_mongoFixture.GetCollectionName(), id);

            result.IsSuccessStatusCode.Should().BeTrue();
            result.Content.Value<string>("name").Should().Be("Mr. Awesome");
            result.Content.Value<string>("id").Should().Be(id);
        }
        
        [Fact]
        public async Task GetDocumentByStringId_WithNonExistingId_ReturnsBadRequest()
        {
            var result = await _functionsApi.GetDocumentByStringId(_mongoFixture.GetCollectionName(), Guid.NewGuid().ToString());

            result.ResultStatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.ErrorMessage.Should().Be("Document not found");
        }

        public void Dispose()
        {
            _mongoFixture.Dispose();
        }
    }
}