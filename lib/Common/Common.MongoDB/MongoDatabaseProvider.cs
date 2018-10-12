using Common.ComponentModel;
using MongoDB.Driver;

namespace Common.MongoDB
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class MongoDatabaseProvider : ComponentBase, IMongoDatabaseProvider
    {
        public IMongoDatabase MongoDatabase { get; }

        public MongoDatabaseProvider(string url)
        {
            var mongoUrl = new MongoUrl(url);
            var mongoClient = new MongoClient(mongoUrl);

            MongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }
    }
}