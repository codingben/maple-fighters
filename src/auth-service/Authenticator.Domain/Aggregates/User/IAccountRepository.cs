using Authenticator.Domain.Repository;

namespace Authenticator.Domain.Aggregates.User
{
    public interface IAccountRepository : IRepository<IAccount, string>
    {
        // Left blank intentionally
    }
}