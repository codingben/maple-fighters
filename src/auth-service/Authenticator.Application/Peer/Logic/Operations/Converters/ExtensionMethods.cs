using System;
using Authenticator.Common;
using Authenticator.Domain.Aggregates.User.Services;

namespace Authenticator.Application.Peer.Logic.Operations.Converters
{
    public static class ExtensionMethods
    {
        public static LoginStatus ToLoginStatus(
            this AuthenticationStatus authenticationStatus)
        {
            LoginStatus loginStatus;

            switch (authenticationStatus)
            {
                case AuthenticationStatus.Authenticated:
                {
                    loginStatus = LoginStatus.Succeed;
                    break;
                }

                case AuthenticationStatus.NotFound:
                {
                    loginStatus = LoginStatus.WrongEmail;
                    break;
                }

                case AuthenticationStatus.WrongPassword:
                {
                    loginStatus = LoginStatus.WrongPassword;
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return loginStatus;
        }

        public static RegistrationStatus ToRegistrationStatus(
            this AccountCreationStatus accountCreationStatus)
        {
            RegistrationStatus registrationStatus;

            switch (accountCreationStatus)
            {
                case AccountCreationStatus.Succeed:
                {
                    registrationStatus = RegistrationStatus.Created;
                    break;
                }

                case AccountCreationStatus.EmailExists:
                {
                    registrationStatus = RegistrationStatus.EmailAlreadyInUse;
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return registrationStatus;
        }
    }
}