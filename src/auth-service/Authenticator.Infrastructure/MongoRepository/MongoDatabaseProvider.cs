using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Authenticator.Infrastructure.MongoRepository
{
    /// <summary>
    /// Provides the mongo database.
    /// </summary>
    public class MongoDatabaseProvider : IDatabaseProvider
    {
        private readonly IMongoDatabase database;

        public MongoDatabaseProvider(string url)
        {
            var mongoUrl = new MongoUrl(url);
            var mongoClient = new MongoClient(mongoUrl);
            database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            Ping(url);
        }

        /// <summary>
        /// Force the connection to be established.
        /// </summary>
        private void Ping(string url)
        {
            try
            {
                var pingCommand = new BsonDocument("ping", 1);
                database.RunCommand<BsonDocument>(pingCommand);
            }
            catch (Exception exception)
            {
                throw new Exception($"Failed to connect to MongoDB server at {url}. {exception.Message}", exception);
            }
        }

        public IMongoDatabase Provide()
        {
            return database;
        }
    }
}