namespace Mallow.Azure.WebJobs.Extensions.Mongo.Connection
{
    internal interface IMongoCollectionFactory
    {
        IBsonCollection Create(ConnectionSettings connectionSettings);
    }
}