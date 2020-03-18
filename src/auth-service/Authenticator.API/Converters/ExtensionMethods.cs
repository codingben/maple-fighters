using Authenticator.API.Services;
using Authenticator.Domain.Aggregates.User.Services;

namespace Authenticator.API.Converters
{
    public static class ExtensionMethods
    {
        public static LoginResponse.Types.LoginStatus ToLoginStatus(
            this AuthenticationStatus authenticationStatus)
        {
            var loginStatus = LoginResponse.Types.LoginStatus.Failed;

            switch (authenticationStatus)
            {
                case AuthenticationStatus.Failed:
                {
                    loginStatus = LoginResponse.Types.LoginStatus.Failed;
                    break;
                }

                case AuthenticationStatus.Authenticated:
                {
                    loginStatus = LoginResponse.Types.LoginStatus.Succeed;
                    break;
                }

                case AuthenticationStatus.NotFound:
                {
                    loginStatus = LoginResponse.Types.LoginStatus.WrongEmail;
                    break;
                }

                case AuthenticationStatus.WrongPassword:
                {
                    loginStatus = LoginResponse.Types.LoginStatus.WrongPassword;
                    break;
                }
            }

            return loginStatus;
        }

        public static RegisterResponse.Types.RegistrationStatus ToRegistrationStatus(
            this AccountCreationStatus accountCreationStatus)
        {
            var registrationStatus = 
                RegisterResponse.Types.RegistrationStatus.Failed;

            switch (accountCreationStatus)
            {
                case AccountCreationStatus.Failed:
                {
                    registrationStatus = 
                        RegisterResponse.Types.RegistrationStatus.Failed;
                    break;
                }

                case AccountCreationStatus.Succeed:
                {
                    registrationStatus = 
                        RegisterResponse.Types.RegistrationStatus.Created;
                    break;
                }

                case AccountCreationStatus.EmailExists:
                {
                    registrationStatus = 
                        RegisterResponse.Types.RegistrationStatus.EmailAlreadyInUse;
                    break;
                }
            }

            return registrationStatus;
        }
    }
}