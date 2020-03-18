using Authenticator.Domain.Aggregates.User.Services;

namespace Authenticator.API.Controllers
{
    public interface ILoginController
    {
        AuthenticationStatus Login(string userName, string password);
    }
}