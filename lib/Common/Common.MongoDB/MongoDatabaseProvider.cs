using Common.ComponentModel;
using MongoDB.Driver;

namespace Common.MongoDB
{
    /// <summary>
    /// Provides the mongo database.
    /// </summary>
    [ComponentSettings(ExposedState.Unexposable)]
    public class MongoDatabaseProvider : ComponentBase, IMongoDatabaseProvider
    {
        private readonly IMongoDatabase database;

        public MongoDatabaseProvider(string url)
        {
            var mongoUrl = new MongoUrl(url);
            var mongoClient = new MongoClient(mongoUrl);

            database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase Provide()
        {
            return database;
        }
    }
}