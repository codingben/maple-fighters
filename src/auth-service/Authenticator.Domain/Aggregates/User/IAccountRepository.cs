using Authenticator.Domain.Repository;
using Authenticator.Infrastructure;

namespace Authenticator.Domain.Aggregates.User
{
    public interface IAccountRepository : IRepository<Account, string>
    {
        // Left blank intentionally
    }
}