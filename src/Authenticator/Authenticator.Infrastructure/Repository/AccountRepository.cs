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

        protected override IMongoCollection<Account> GetCollection()
        {
            var databaseProvider = Components.Get<IMongoDatabaseProvider>()
                .AssertNotNull();
            var collection =
                databaseProvider.MongoDatabase.GetCollection<Account>(
                    CollectionName);
            return collection;
        }
    }
}