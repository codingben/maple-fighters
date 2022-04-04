namespace Authenticator.Domain.Aggregates.User.Services
{
    public interface ILoginService
    {
        IAccount FindAccount(string email);
    }
}