using Authenticator.API.Datas;
using Authenticator.API.Validators;
using Authenticator.Domain.Aggregates.User.Services;

namespace Authenticator.API.Controllers
{
    public class LoginController : ILoginController
    {
        private readonly ILoginService loginService;
        private readonly AuthenticationDataValidator authenticationDataValidator;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;

            authenticationDataValidator = new AuthenticationDataValidator();
        }

        public AuthenticationStatus Login(AuthenticationData authenticationData)
        {
            var authenticationStatus = AuthenticationStatus.NotFound;

            // TODO: Get errors if invalid
            var validationResult =
                authenticationDataValidator.Validate(authenticationData);
            if (validationResult.IsValid)
            {
                var email = authenticationData.Email;
                var password = authenticationData.Password;

                authenticationStatus =
                    loginService.Authenticate(email, password);
            }

            return authenticationStatus;
        }
    }
}