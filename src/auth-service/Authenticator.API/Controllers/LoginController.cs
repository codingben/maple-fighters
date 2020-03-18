using Authenticator.Domain.Aggregates.User.Services;

namespace Authenticator.API.Controllers
{
    public class LoginController : ILoginController
    {
        private ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public AuthenticationStatus Login(string userName, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}