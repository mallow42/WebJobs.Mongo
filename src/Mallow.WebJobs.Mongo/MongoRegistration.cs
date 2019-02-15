using Mallow.Azure.WebJobs.Extensions.Mongo.Collector;
using Mallow.Azure.WebJobs.Extensions.Mongo.Connection;
using Mallow.Azure.WebJobs.Extensions.Mongo.Converters;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Mallow.Azure.WebJobs.Extensions.Mongo
{
    internal class MongoRegistration : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var mongoCollectionFactory = new MongoCollectionFactory();
            
            var rule = context.AddBindingRule<MongoAttribute>();
            rule.BindToCollector<OpenType.Poco>(typeof(MongoCollectorBuilder<>), mongoCollectionFactory);
            rule.BindToInput<OpenType.Poco>(typeof(MongoDocumentConverter<>), mongoCollectionFactory);
        }
    }
}