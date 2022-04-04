using MongoDB.Driver;

namespace Authenticator.Infrastructure.MongoRepository
{
    /// <summary>
    /// Represents the provider of the mongo database.
    /// </summary>
    public interface IDatabaseProvider
    {
        IMongoDatabase Provide();
    }
}