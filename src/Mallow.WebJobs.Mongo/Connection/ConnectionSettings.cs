namespace Mallow.Azure.WebJobs.Extensions.Mongo.Connection
{
    internal class ConnectionSettings
    {
        public string ConnectionString { get; }
        
        public string DatabaseId { get; }
        
        public string CollectionId { get; }

        public ConnectionSettings(string connectionString, string databaseId, string collectionId)
        {
            ConnectionString = connectionString;
            DatabaseId = databaseId;
            CollectionId = collectionId;
        }
    }
}