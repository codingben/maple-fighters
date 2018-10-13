namespace Authenticator.Domain.Aggregates.User.Services
{
    public interface ILoginService
    {
        AuthenticationStatus Authenticate(string email, string password);
    }
}