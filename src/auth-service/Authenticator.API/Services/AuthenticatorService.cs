using System.Threading.Tasks;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Grpc.Core;

namespace Authenticator.API.Services
{
    public class AuthenticatorService : Authenticator.AuthenticatorBase
    {
        private readonly ILoginService loginService;
        private readonly IRegistrationService registrationService;

        public AuthenticatorService(ILoginService loginService, IRegistrationService registrationService)
        {
            this.loginService = loginService;
            this.registrationService = registrationService;
        }

        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            // TODO: Validate parameters
            var authenticationStatus = loginService.Authenticate(
                request.Email,
                request.Password);

            var loginStatus = LoginResponse.Types.LoginStatus.Failed;

            // TODO: Use convertor
            switch (authenticationStatus)
            {
                case AuthenticationStatus.Authenticated:
                {
                    loginStatus = LoginResponse.Types.LoginStatus.Succeed;
                    break;
                }

                case AuthenticationStatus.NotFound:
                {
                    loginStatus = LoginResponse.Types.LoginStatus.Failed;
                    break;
                }

                case AuthenticationStatus.WrongPassword:
                {
                    loginStatus = LoginResponse.Types.LoginStatus.WrongPassword;
                    break;
                }
            }

            return Task.FromResult(new LoginResponse { LoginStatus = loginStatus });
        }

        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            // TODO: Validate parameters
            var account = AccountFactory.CreateAccount(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName);
            var accountCreationStatus =
                registrationService.CreateAccount(account);

            // TODO: Use converter
            var registrationStatus =
                RegisterResponse.Types.RegistrationStatus.Failed;

            switch (accountCreationStatus)
            {
                case AccountCreationStatus.Succeed:
                {
                    registrationStatus = RegisterResponse.Types.RegistrationStatus.Created;
                    break;
                }

                case AccountCreationStatus.EmailExists:
                {
                    registrationStatus = RegisterResponse.Types.RegistrationStatus.EmailAlreadyInUse;
                    break;
                }
            }

            return Task.FromResult(new RegisterResponse { RegistrationStatus = registrationStatus });
        }
    }
}