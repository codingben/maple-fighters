using Common.MongoDB;

namespace Authenticator.Domain.Aggregates.User
{
    public interface IAccountRepository : IMongoRepository<Account>
    {
        // Left blank intentionally
    }
}