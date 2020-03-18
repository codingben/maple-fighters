using System.Threading.Tasks;
using Authenticator.API.Controllers;
using Authenticator.API.Converters;
using Authenticator.API.Datas;
using Grpc.Core;

namespace Authenticator.API.Services
{
    public class AuthenticatorService : Authenticator.AuthenticatorBase
    {
        private readonly ILoginController loginController;
        private readonly IRegistrationController registrationController;

        public AuthenticatorService(
            ILoginController loginController,
            IRegistrationController registrationController)
        {
            this.loginController = loginController;
            this.registrationController = registrationController;
        }

        public override Task<LoginResponse> Login(
            LoginRequest request,
            ServerCallContext context)
        {
            var authenticationData = new AuthenticationData(
                request.Email,
                request.Password);
            var authenticationStatus =
                loginController.Login(authenticationData);

            return Task.FromResult(new LoginResponse
            {
                LoginStatus = authenticationStatus.ToLoginStatus()
            });
        }

        public override Task<RegisterResponse> Register(
            RegisterRequest request,
            ServerCallContext context)
        {
            var registrationData = new RegistrationData(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName);
            var accountCreationStatus =
                registrationController.Register(registrationData);

            return Task.FromResult(new RegisterResponse
            {
                RegistrationStatus =
                    accountCreationStatus.ToRegistrationStatus()
            });
        }
    }
}