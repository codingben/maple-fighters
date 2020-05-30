using Authenticator.API.Services;
using Authenticator.Domain.Aggregates.User.Services;

namespace Authenticator.API.Converters
{
    public static class ExtensionMethods
    {
        public static LoginResponse.Types.LoginStatus ToLoginStatus(
            this AuthenticationStatus authenticationStatus)
        {
            switch (authenticationStatus)
            {
                case AuthenticationStatus.Failed:
                {
                    return LoginResponse.Types.LoginStatus.Failed;
                }

                case AuthenticationStatus.Authenticated:
                {
                    return LoginResponse.Types.LoginStatus.Succeed;
                }

                case AuthenticationStatus.NotFound:
                {
                    return LoginResponse.Types.LoginStatus.WrongEmail;
                }

                case AuthenticationStatus.WrongPassword:
                {
                    return LoginResponse.Types.LoginStatus.WrongPassword;
                }
            }

            return LoginResponse.Types.LoginStatus.Failed;
        }

        public static RegisterResponse.Types.RegistrationStatus ToRegistrationStatus(
            this AccountCreationStatus accountCreationStatus)
        {
            switch (accountCreationStatus)
            {
                case AccountCreationStatus.Failed:
                {
                    return RegisterResponse.Types.RegistrationStatus.Failed;
                }

                case AccountCreationStatus.Succeed:
                {
                    return RegisterResponse.Types.RegistrationStatus.Created;
                }

                case AccountCreationStatus.EmailExists:
                {
                    return RegisterResponse.Types.RegistrationStatus.EmailAlreadyInUse;
                }
            }

            return RegisterResponse.Types.RegistrationStatus.Failed;
        }
    }
}