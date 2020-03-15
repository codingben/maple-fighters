using Authenticator.Domain.Aggregates.User;
using Common.ComponentModel;
using Common.MongoDB;
using CommonTools.Log;
using MongoDB.Driver;

namespace Authenticator.Infrastructure.Repository
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class AccountRepository : MongoRepositoryBase<Account>, IAccountRepository
    {
        private const string CollectionName = "accounts";

        private IMongoDatabase mongoDatabase;

        protected override void OnAwake()
        {
            base.OnAwake();

            var mongoDatabaseProvider = Components.Get<IMongoDatabaseProvider>()
                .AssertNotNull();
            mongoDatabase = mongoDatabaseProvider.Provide();
        }

        protected override IMongoCollection<Account> GetCollection()
        {
            return mongoDatabase.GetCollection<Account>(CollectionName);
        }
    }
}