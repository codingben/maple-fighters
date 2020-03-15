using Common.Repository.Interfaces;

namespace Authenticator.Domain.Aggregates.User
{
    public interface IAccountRepository : IRepository<Account, string>
    {
        // Left blank intentionally
    }
}