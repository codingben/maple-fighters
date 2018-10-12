using MongoDB.Driver;

namespace Common.MongoDB
{
    public interface IMongoDatabaseProvider
    {
        IMongoDatabase MongoDatabase { get; }
    }
}