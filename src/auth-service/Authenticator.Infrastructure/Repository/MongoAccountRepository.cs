using Authenticator.Domain.Aggregates.User;
using Common.MongoDB;
using MongoDB.Driver;

namespace Authenticator.Infrastructure.Repository
{
    public class MongoAccountRepository : MongoRepository<Account>, IAccountRepository
    {
        private readonly IMongoDatabase mongoDatabase;

        public MongoAccountRepository(IDatabaseProvider databaseProvider)
        {
            mongoDatabase = databaseProvider.Provide();
        }

        protected override IMongoCollection<Account> GetCollection()
        {
            return mongoDatabase.GetCollection<Account>(name: "accounts");
        }
    }
}