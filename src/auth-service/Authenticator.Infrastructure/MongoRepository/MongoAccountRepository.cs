using Authenticator.Domain.Aggregates.User;
using MongoDB.Driver;

namespace Authenticator.Infrastructure.MongoRepository
{
    public class MongoAccountRepository : MongoRepository<IAccount>, IAccountRepository
    {
        private readonly IMongoDatabase mongoDatabase;

        public MongoAccountRepository(IDatabaseProvider databaseProvider)
        {
            mongoDatabase = databaseProvider.Provide();
        }

        protected override IMongoCollection<IAccount> GetCollection()
        {
            return mongoDatabase.GetCollection<IAccount>(name: "accounts");
        }
    }
}