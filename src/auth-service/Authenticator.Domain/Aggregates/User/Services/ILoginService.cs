namespace Authenticator.Domain.Aggregates.User.Services
{
    public interface ILoginService
    {
        Account FindAccount(string email);
    }
}