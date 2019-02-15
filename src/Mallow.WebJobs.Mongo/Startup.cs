using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(Mallow.Azure.WebJobs.Extensions.Mongo.Startup))]

namespace Mallow.Azure.WebJobs.Extensions.Mongo
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<MongoRegistration>();
        }
    }
}
