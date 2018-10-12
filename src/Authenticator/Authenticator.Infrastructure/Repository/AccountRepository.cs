using Authenticator.Domain.Aggregates.User;
using Common.MongoDB;
using CommonTools.Log;
using MongoDB.Driver;

namespace Authenticator.Infrastructure.Repository
{
    public class AccountRepository : MongoRepositoryBase<Account>
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