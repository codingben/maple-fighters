namespace Authenticator.Domain.Aggregates.User.Services
{
    public interface ILoginService
    {
        Account Authenticate(string email, string password);
    }
}