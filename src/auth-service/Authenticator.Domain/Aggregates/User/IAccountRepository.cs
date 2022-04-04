using Common.Repository.Interfaces;

namespace Authenticator.Domain.Aggregates.User
{
    public interface IAccountRepository : IRepository<IAccount, string>
    {
        // Left blank intentionally
    }
}