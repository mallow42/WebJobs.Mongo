using System;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Microsoft.Azure.WebJobs.Description;

namespace Mallow.Azure.WebJobs.Extensions.Mongo
{
    /// <summary>
    /// Attribute used to bind to MongoDB collection.
    /// </summary>
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public sealed class MongoAttribute : Attribute
    {
        /// <summary>
        /// Connection string to MongoDB instance.        
        /// May include binding parameters.
        /// </summary>
        [AutoResolve]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Database ID.        
        /// May include binding parameters.
        /// </summary>
        [AutoResolve]
        public string DatabaseId { get; set; }

        /// <summary>
        /// Collection ID.        
        /// May include binding parameters.
        /// </summary>
        [AutoResolve]
        public string CollectionId { get; set; }

        /// <summary>
        /// Id of document when used as input binding.        
        /// May include binding parameters.
        /// </summary>
        [AutoResolve]
        public string Id { get; set; }

        /// <summary>
        /// Sets mode that is used when creating/replacing document.
        /// </summary>
        public InsertMode Mode { get; set; } = InsertMode.Create;
        
        /// <summary>
        /// Filter used to obtain documents.        
        /// May include binding parameters.
        /// </summary>
        [AutoResolve]
        public string Filter { get; set; }

        internal ConnectionSettings ToConnectionSettings()
        {
            return new ConnectionSettings(ConnectionString, DatabaseId, CollectionId);
        }
    }
}